// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Admin/IdentityService.proto
// </auto-generated>
// Original file comments:
//
//
//
#pragma warning disable 1591
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using grpc = global::Grpc.Core;

namespace Axiverse.Services.Proto {
  public static partial class IdentityService
  {
    static readonly string __ServiceName = "IdentityService";

    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.ValidateIdentityRequest> __Marshaller_ValidateIdentityRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.ValidateIdentityRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.ValidateIdentityResponse> __Marshaller_ValidateIdentityResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.ValidateIdentityResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.CreateIdentityRequest> __Marshaller_CreateIdentityRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.CreateIdentityRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.CreateIdentityResponse> __Marshaller_CreateIdentityResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.CreateIdentityResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.DeleteIdentityRequest> __Marshaller_DeleteIdentityRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.DeleteIdentityRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.DeleteIdentityResponse> __Marshaller_DeleteIdentityResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.DeleteIdentityResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.GetIdentityRequest> __Marshaller_GetIdentityRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.GetIdentityRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.GetIdentityResponse> __Marshaller_GetIdentityResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.GetIdentityResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::Axiverse.Services.Proto.ValidateIdentityRequest, global::Axiverse.Services.Proto.ValidateIdentityResponse> __Method_ValidateIdentity = new grpc::Method<global::Axiverse.Services.Proto.ValidateIdentityRequest, global::Axiverse.Services.Proto.ValidateIdentityResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ValidateIdentity",
        __Marshaller_ValidateIdentityRequest,
        __Marshaller_ValidateIdentityResponse);

    static readonly grpc::Method<global::Axiverse.Services.Proto.CreateIdentityRequest, global::Axiverse.Services.Proto.CreateIdentityResponse> __Method_CreateIdentity = new grpc::Method<global::Axiverse.Services.Proto.CreateIdentityRequest, global::Axiverse.Services.Proto.CreateIdentityResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CreateIdentity",
        __Marshaller_CreateIdentityRequest,
        __Marshaller_CreateIdentityResponse);

    static readonly grpc::Method<global::Axiverse.Services.Proto.DeleteIdentityRequest, global::Axiverse.Services.Proto.DeleteIdentityResponse> __Method_DeleteIdentity = new grpc::Method<global::Axiverse.Services.Proto.DeleteIdentityRequest, global::Axiverse.Services.Proto.DeleteIdentityResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "DeleteIdentity",
        __Marshaller_DeleteIdentityRequest,
        __Marshaller_DeleteIdentityResponse);

    static readonly grpc::Method<global::Axiverse.Services.Proto.GetIdentityRequest, global::Axiverse.Services.Proto.GetIdentityResponse> __Method_GetIdentity = new grpc::Method<global::Axiverse.Services.Proto.GetIdentityRequest, global::Axiverse.Services.Proto.GetIdentityResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetIdentity",
        __Marshaller_GetIdentityRequest,
        __Marshaller_GetIdentityResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Axiverse.Services.Proto.IdentityServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of IdentityService</summary>
    public abstract partial class IdentityServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Axiverse.Services.Proto.ValidateIdentityResponse> ValidateIdentity(global::Axiverse.Services.Proto.ValidateIdentityRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Axiverse.Services.Proto.CreateIdentityResponse> CreateIdentity(global::Axiverse.Services.Proto.CreateIdentityRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Axiverse.Services.Proto.DeleteIdentityResponse> DeleteIdentity(global::Axiverse.Services.Proto.DeleteIdentityRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Axiverse.Services.Proto.GetIdentityResponse> GetIdentity(global::Axiverse.Services.Proto.GetIdentityRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for IdentityService</summary>
    public partial class IdentityServiceClient : grpc::ClientBase<IdentityServiceClient>
    {
      /// <summary>Creates a new client for IdentityService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public IdentityServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for IdentityService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public IdentityServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected IdentityServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected IdentityServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Axiverse.Services.Proto.ValidateIdentityResponse ValidateIdentity(global::Axiverse.Services.Proto.ValidateIdentityRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return ValidateIdentity(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Axiverse.Services.Proto.ValidateIdentityResponse ValidateIdentity(global::Axiverse.Services.Proto.ValidateIdentityRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ValidateIdentity, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.ValidateIdentityResponse> ValidateIdentityAsync(global::Axiverse.Services.Proto.ValidateIdentityRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return ValidateIdentityAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.ValidateIdentityResponse> ValidateIdentityAsync(global::Axiverse.Services.Proto.ValidateIdentityRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ValidateIdentity, null, options, request);
      }
      public virtual global::Axiverse.Services.Proto.CreateIdentityResponse CreateIdentity(global::Axiverse.Services.Proto.CreateIdentityRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CreateIdentity(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Axiverse.Services.Proto.CreateIdentityResponse CreateIdentity(global::Axiverse.Services.Proto.CreateIdentityRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CreateIdentity, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.CreateIdentityResponse> CreateIdentityAsync(global::Axiverse.Services.Proto.CreateIdentityRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return CreateIdentityAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.CreateIdentityResponse> CreateIdentityAsync(global::Axiverse.Services.Proto.CreateIdentityRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CreateIdentity, null, options, request);
      }
      public virtual global::Axiverse.Services.Proto.DeleteIdentityResponse DeleteIdentity(global::Axiverse.Services.Proto.DeleteIdentityRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return DeleteIdentity(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Axiverse.Services.Proto.DeleteIdentityResponse DeleteIdentity(global::Axiverse.Services.Proto.DeleteIdentityRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_DeleteIdentity, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.DeleteIdentityResponse> DeleteIdentityAsync(global::Axiverse.Services.Proto.DeleteIdentityRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return DeleteIdentityAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.DeleteIdentityResponse> DeleteIdentityAsync(global::Axiverse.Services.Proto.DeleteIdentityRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_DeleteIdentity, null, options, request);
      }
      public virtual global::Axiverse.Services.Proto.GetIdentityResponse GetIdentity(global::Axiverse.Services.Proto.GetIdentityRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetIdentity(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Axiverse.Services.Proto.GetIdentityResponse GetIdentity(global::Axiverse.Services.Proto.GetIdentityRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetIdentity, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.GetIdentityResponse> GetIdentityAsync(global::Axiverse.Services.Proto.GetIdentityRequest request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetIdentityAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.GetIdentityResponse> GetIdentityAsync(global::Axiverse.Services.Proto.GetIdentityRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetIdentity, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override IdentityServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new IdentityServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(IdentityServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_ValidateIdentity, serviceImpl.ValidateIdentity)
          .AddMethod(__Method_CreateIdentity, serviceImpl.CreateIdentity)
          .AddMethod(__Method_DeleteIdentity, serviceImpl.DeleteIdentity)
          .AddMethod(__Method_GetIdentity, serviceImpl.GetIdentity).Build();
    }

  }
}
#endregion