﻿using System;
using System.Diagnostics;
using ENTech.Store.Infrastructure;
using ENTech.Store.Infrastructure.Database.EF6;
using ENTech.Store.Infrastructure.Database.EF6.UnitOfWork;
using ENTech.Store.Infrastructure.Services.Commands;
using ENTech.Store.Infrastructure.Services.Errors.ResponseErrors;
using ENTech.Store.Infrastructure.Services.Requests;
using ENTech.Store.Infrastructure.Services.Responses;
using ENTech.Store.Infrastructure.Services.Responses.Statuses;
using ENTech.Store.Services.AuthenticationModule.Dtos;
using ENTech.Store.Services.CommandService.Definition;
using ENTech.Store.Services.Misc;

namespace ENTech.Store.Services.CommandService
{
	public class ExternalCommandService :
		CommandServiceBase,
		IExternalCommandService
	{
		private readonly IInternalCommandService _internalCommandService;

		protected IInternalCommandService InternalCommandService { get { return _internalCommandService; } }

		public ExternalCommandService(ICommandFactory commandFactory)
			: base(commandFactory)
		{
			_internalCommandService = new InternalCommandService(commandFactory);
		}

		public IResponseStatus<TResponse>  Execute<TRequest, TResponse, TCommand>(TRequest request)
			where TRequest : IRequest
			where TResponse : IResponse, new()
			where TCommand : ICommand<TRequest, TResponse>
		{
			var unitOfWork = IoC.Resolve<IUnitOfWork>();

			var stopwatch = new Stopwatch();

			IResponseStatus<TResponse> responseStatus;

			AuthenticateResult authenticateResult = new AuthenticateResult();

			try
			{
				stopwatch.Start();

				var command = CommandFactory.Create<TCommand>();

				if (command.RequiresTransaction)
				{
					unitOfWork.BeginTransaction();
				}

				authenticateResult = Authenticate(unitOfWork, request);

				if (authenticateResult.IsSuccess == false)
				{
					var error = new UserNotAuthenticatedResponseError();
					
					return new ErrorResponseStatus<TResponse>(error);
				}

				LimitDbContext(request, unitOfWork.DbContext);
				try
				{
					responseStatus = TryExecute<TRequest, TResponse, TCommand>(request, command);

					if (responseStatus is OkResponseStatus<TResponse>)
					{
					unitOfWork.SaveChanges();

					if (command.RequiresTransaction)
					{					
						unitOfWork.CompleteTransaction();
					}

						AfterExecute(request, ((OkResponseStatus<TResponse>) responseStatus).Response, command);
					}
				}
				catch// (Exception e)
				{
					if (command.RequiresTransaction)
					{
						unitOfWork.RollbackTransaction();
					}

					//ErrorLogUtils.AddError(e);

					var error = new InternalServerResponseError();
					responseStatus = new ErrorResponseStatus<TResponse>(error);
				}

				unitOfWork.Dispose();
				
				return responseStatus;
			}
			catch (Exception e)
			{
				//ErrorLogUtils.AddError(e);
				var error = new InternalServerResponseError(e.Message);
				responseStatus = new ErrorResponseStatus<TResponse>(error);
				return responseStatus;
			}
			finally
			{
				if (unitOfWork != null && unitOfWork.IsDisposed == false)
					unitOfWork.Dispose();

				stopwatch.Stop();

				//SaveApiLogEntry(stopwatch.ElapsedMilliseconds, response ?? errResponse, request, authenticateResult.Partner);
			}
		}

		protected AuthenticateResult Authenticate(IUnitOfWork unitOfWork, IRequest request)
		{
			//var result = _internalCommandService
			//	.Execute
			//	<SecureRequestBase<TSecurity>, AuthenticateApiKeyResponse,
			//		AuthenticateApiKeyCommand<SecureRequestBase<TSecurity>>>(request);

			//if (result.IsAuthenticated)
			//{
			//	var partner = result.Partner;
			//	var internalAuthenticateResult = AuthenticateInternal(unitOfWork, request);
			//	return new AuthenticateResult
			//	{
			//		IsSuccess = internalAuthenticateResult.IsSuccess,
			//		ErrorMessage = internalAuthenticateResult.ErrorMessage,
			//		Partner = internalAuthenticateResult.IsSuccess ? partner : null
			//	};
			//}

			return new AuthenticateResult
			{
				IsSuccess = true
			};
		}


		protected virtual InternalAuthenticateResult AuthenticateInternal(IUnitOfWork unitOfWork, IRequest request)
		{
			return new InternalAuthenticateResult
			{
				IsSuccess = true
			};
		}

		private void LimitDbContext(IRequest request, IDbContext dbContext)
		{
			//USE Thread.CurrentPrincipal to understand how to limit	
		}

		private void SaveApiLogEntry<TRequest, TResponse>(decimal duration, TResponse response, TRequest request, PartnerDto partner)
			where TResponse : IResponse, new()
			where TRequest : IRequest
		{
			//try
			//{
			//	string fullUrl = String.Empty;
			//	string route = String.Empty;
			//	string query = String.Empty;
			//	string headers = String.Empty;
			//	string jsonRequest = String.Empty;
			//	string callerIp = String.Empty;

			//	string jsonResponse = JsonConvert.SerializeObject(response);

			//	if (HttpContext.Current != null)
			//	{
			//		fullUrl = HttpContext.Current.Request.Url.OriginalString;

			//		route = HttpContext.Current.Request.Url.AbsolutePath;

			//		var sb = new StringBuilder();

			//		for (int i = 0; i < HttpContext.Current.Request.Headers.Count; i++)
			//			sb.AppendFormat("{0}={1};", HttpContext.Current.Request.Headers.Keys[i],
			//				HttpContext.Current.Request.Headers[i].ToString());

			//		headers = sb.ToString();

			//		query = HttpContext.Current.Request.QueryString.ToString();

			//		using (var reader = new StreamReader(HttpContext.Current.Request.InputStream))
			//		{
			//			try
			//			{
			//				HttpContext.Current.Request.InputStream.Position = 0;
			//				jsonRequest = reader.ReadToEnd();
			//			}
			//			catch (Exception ex)
			//			{
			//			}
			//			finally
			//			{
			//				HttpContext.Current.Request.InputStream.Position = 0;
			//			}
			//		}

			//		callerIp = HttpContext.Current.Request.UserHostAddress;
			//	}


			//	if (HttpContext.Current == null || !HttpContext.Current.Request.Url.OriginalString.Contains(EnvSettings.ApiUrl))
			//		jsonRequest = JsonConvert.SerializeObject(request);


			//	int? partnerId = null;
			//	string partnerName = String.Empty;

			//	if (partner != null)
			//	{
			//		partnerId = partner.Id;
			//		partnerName = partner.Name;
			//	}

			//	ErrorLogUtils.AddApiLogEntry(partnerId, partnerName, (int) EnvSettings.EnvType, EnvSettings.EnvType.GetStringValue(),
			//		fullUrl, route, query, headers, jsonRequest, jsonResponse, duration, callerIp);
			//}
			//catch (Exception ex)
			//{
			//	ErrorLogUtils.AddError(ex);
			//}
		}


		protected class InternalAuthenticateResult
		{
			public bool IsSuccess { get; set; }
			public string ErrorMessage { get; set; }
		}
	}
}