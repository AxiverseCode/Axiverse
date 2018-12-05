// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Admin/AliasService.proto
// </auto-generated>
// Original file comments:
// Alias Service
//
// Administrative service for resolving aliases to addresses.
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Axiverse.Services.Proto {
  public static partial class AliasService
  {
    static readonly string __ServiceName = "AliasService";

    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.ResolveAliasRequest> __Marshaller_ResolveAliasRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.ResolveAliasRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.ResolveAliasResponse> __Marshaller_ResolveAliasResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.ResolveAliasResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.CreateAliasRequest> __Marshaller_CreateAliasRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.CreateAliasRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.CreateAliasResponse> __Marshaller_CreateAliasResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.CreateAliasResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.DeleteAliasRequest> __Marshaller_DeleteAliasRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.DeleteAliasRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Axiverse.Services.Proto.DeleteAliasResponse> __Marshaller_DeleteAliasResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Axiverse.Services.Proto.DeleteAliasResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::Axiverse.Services.Proto.ResolveAliasRequest, global::Axiverse.Services.Proto.ResolveAliasResponse> __Method_ResolveAlias = new grpc::Method<global::Axiverse.Services.Proto.ResolveAliasRequest, global::Axiverse.Services.Proto.ResolveAliasResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ResolveAlias",
        __Marshaller_ResolveAliasRequest,
        __Marshaller_ResolveAliasResponse);

    static readonly grpc::Method<global::Axiverse.Services.Proto.CreateAliasRequest, global::Axiverse.Services.Proto.CreateAliasResponse> __Method_CreateAlias = new grpc::Method<global::Axiverse.Services.Proto.CreateAliasRequest, global::Axiverse.Services.Proto.CreateAliasResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CreateAlias",
        __Marshaller_CreateAliasRequest,
        __Marshaller_CreateAliasResponse);

    static readonly grpc::Method<global::Axiverse.Services.Proto.DeleteAliasRequest, global::Axiverse.Services.Proto.DeleteAliasResponse> __Method_DeleteAlias = new grpc::Method<global::Axiverse.Services.Proto.DeleteAliasRequest, global::Axiverse.Services.Proto.DeleteAliasResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "DeleteAlias",
        __Marshaller_DeleteAliasRequest,
        __Marshaller_DeleteAliasResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Axiverse.Services.Proto.AliasServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of AliasService</summary>
    public abstract partial class AliasServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Axiverse.Services.Proto.ResolveAliasResponse> ResolveAlias(global::Axiverse.Services.Proto.ResolveAliasRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Axiverse.Services.Proto.CreateAliasResponse> CreateAlias(global::Axiverse.Services.Proto.CreateAliasRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Axiverse.Services.Proto.DeleteAliasResponse> DeleteAlias(global::Axiverse.Services.Proto.DeleteAliasRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for AliasService</summary>
    public partial class AliasServiceClient : grpc::ClientBase<AliasServiceClient>
    {
      /// <summary>Creates a new client for AliasService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public AliasServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for AliasService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public AliasServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected AliasServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected AliasServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Axiverse.Services.Proto.ResolveAliasResponse ResolveAlias(global::Axiverse.Services.Proto.ResolveAliasRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ResolveAlias(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Axiverse.Services.Proto.ResolveAliasResponse ResolveAlias(global::Axiverse.Services.Proto.ResolveAliasRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ResolveAlias, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.ResolveAliasResponse> ResolveAliasAsync(global::Axiverse.Services.Proto.ResolveAliasRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ResolveAliasAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.ResolveAliasResponse> ResolveAliasAsync(global::Axiverse.Services.Proto.ResolveAliasRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ResolveAlias, null, options, request);
      }
      public virtual global::Axiverse.Services.Proto.CreateAliasResponse CreateAlias(global::Axiverse.Services.Proto.CreateAliasRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateAlias(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Axiverse.Services.Proto.CreateAliasResponse CreateAlias(global::Axiverse.Services.Proto.CreateAliasRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CreateAlias, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.CreateAliasResponse> CreateAliasAsync(global::Axiverse.Services.Proto.CreateAliasRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateAliasAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.CreateAliasResponse> CreateAliasAsync(global::Axiverse.Services.Proto.CreateAliasRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CreateAlias, null, options, request);
      }
      public virtual global::Axiverse.Services.Proto.DeleteAliasResponse DeleteAlias(global::Axiverse.Services.Proto.DeleteAliasRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DeleteAlias(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Axiverse.Services.Proto.DeleteAliasResponse DeleteAlias(global::Axiverse.Services.Proto.DeleteAliasRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_DeleteAlias, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.DeleteAliasResponse> DeleteAliasAsync(global::Axiverse.Services.Proto.DeleteAliasRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return DeleteAliasAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Axiverse.Services.Proto.DeleteAliasResponse> DeleteAliasAsync(global::Axiverse.Services.Proto.DeleteAliasRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_DeleteAlias, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override AliasServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new AliasServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(AliasServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_ResolveAlias, serviceImpl.ResolveAlias)
          .AddMethod(__Method_CreateAlias, serviceImpl.CreateAlias)
          .AddMethod(__Method_DeleteAlias, serviceImpl.DeleteAlias).Build();
    }

    /// <summary>Register service method implementations with a service binder. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, AliasServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_ResolveAlias, serviceImpl.ResolveAlias);
      serviceBinder.AddMethod(__Method_CreateAlias, serviceImpl.CreateAlias);
      serviceBinder.AddMethod(__Method_DeleteAlias, serviceImpl.DeleteAlias);
    }

  }
}
#endregion
