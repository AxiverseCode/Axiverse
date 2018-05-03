// GENERATED CODE -- DO NOT EDIT!

// Original file comments:
//
//
//
'use strict';
var grpc = require('grpc');
var Admin_IdentityService_pb = require('../Admin/IdentityService_pb.js');

function serialize_CreateIdentityRequest(arg) {
  if (!(arg instanceof Admin_IdentityService_pb.CreateIdentityRequest)) {
    throw new Error('Expected argument of type CreateIdentityRequest');
  }
  return new Buffer(arg.serializeBinary());
}

function deserialize_CreateIdentityRequest(buffer_arg) {
  return Admin_IdentityService_pb.CreateIdentityRequest.deserializeBinary(new Uint8Array(buffer_arg));
}

function serialize_CreateIdentityResponse(arg) {
  if (!(arg instanceof Admin_IdentityService_pb.CreateIdentityResponse)) {
    throw new Error('Expected argument of type CreateIdentityResponse');
  }
  return new Buffer(arg.serializeBinary());
}

function deserialize_CreateIdentityResponse(buffer_arg) {
  return Admin_IdentityService_pb.CreateIdentityResponse.deserializeBinary(new Uint8Array(buffer_arg));
}

function serialize_DeleteIdentityRequest(arg) {
  if (!(arg instanceof Admin_IdentityService_pb.DeleteIdentityRequest)) {
    throw new Error('Expected argument of type DeleteIdentityRequest');
  }
  return new Buffer(arg.serializeBinary());
}

function deserialize_DeleteIdentityRequest(buffer_arg) {
  return Admin_IdentityService_pb.DeleteIdentityRequest.deserializeBinary(new Uint8Array(buffer_arg));
}

function serialize_DeleteIdentityResponse(arg) {
  if (!(arg instanceof Admin_IdentityService_pb.DeleteIdentityResponse)) {
    throw new Error('Expected argument of type DeleteIdentityResponse');
  }
  return new Buffer(arg.serializeBinary());
}

function deserialize_DeleteIdentityResponse(buffer_arg) {
  return Admin_IdentityService_pb.DeleteIdentityResponse.deserializeBinary(new Uint8Array(buffer_arg));
}

function serialize_GetIdentityRequest(arg) {
  if (!(arg instanceof Admin_IdentityService_pb.GetIdentityRequest)) {
    throw new Error('Expected argument of type GetIdentityRequest');
  }
  return new Buffer(arg.serializeBinary());
}

function deserialize_GetIdentityRequest(buffer_arg) {
  return Admin_IdentityService_pb.GetIdentityRequest.deserializeBinary(new Uint8Array(buffer_arg));
}

function serialize_GetIdentityResponse(arg) {
  if (!(arg instanceof Admin_IdentityService_pb.GetIdentityResponse)) {
    throw new Error('Expected argument of type GetIdentityResponse');
  }
  return new Buffer(arg.serializeBinary());
}

function deserialize_GetIdentityResponse(buffer_arg) {
  return Admin_IdentityService_pb.GetIdentityResponse.deserializeBinary(new Uint8Array(buffer_arg));
}

function serialize_ValidateIdentityRequest(arg) {
  if (!(arg instanceof Admin_IdentityService_pb.ValidateIdentityRequest)) {
    throw new Error('Expected argument of type ValidateIdentityRequest');
  }
  return new Buffer(arg.serializeBinary());
}

function deserialize_ValidateIdentityRequest(buffer_arg) {
  return Admin_IdentityService_pb.ValidateIdentityRequest.deserializeBinary(new Uint8Array(buffer_arg));
}

function serialize_ValidateIdentityResponse(arg) {
  if (!(arg instanceof Admin_IdentityService_pb.ValidateIdentityResponse)) {
    throw new Error('Expected argument of type ValidateIdentityResponse');
  }
  return new Buffer(arg.serializeBinary());
}

function deserialize_ValidateIdentityResponse(buffer_arg) {
  return Admin_IdentityService_pb.ValidateIdentityResponse.deserializeBinary(new Uint8Array(buffer_arg));
}


var IdentityServiceService = exports.IdentityServiceService = {
  validateIdentity: {
    path: '/IdentityService/ValidateIdentity',
    requestStream: false,
    responseStream: false,
    requestType: Admin_IdentityService_pb.ValidateIdentityRequest,
    responseType: Admin_IdentityService_pb.ValidateIdentityResponse,
    requestSerialize: serialize_ValidateIdentityRequest,
    requestDeserialize: deserialize_ValidateIdentityRequest,
    responseSerialize: serialize_ValidateIdentityResponse,
    responseDeserialize: deserialize_ValidateIdentityResponse,
  },
  createIdentity: {
    path: '/IdentityService/CreateIdentity',
    requestStream: false,
    responseStream: false,
    requestType: Admin_IdentityService_pb.CreateIdentityRequest,
    responseType: Admin_IdentityService_pb.CreateIdentityResponse,
    requestSerialize: serialize_CreateIdentityRequest,
    requestDeserialize: deserialize_CreateIdentityRequest,
    responseSerialize: serialize_CreateIdentityResponse,
    responseDeserialize: deserialize_CreateIdentityResponse,
  },
  deleteIdentity: {
    path: '/IdentityService/DeleteIdentity',
    requestStream: false,
    responseStream: false,
    requestType: Admin_IdentityService_pb.DeleteIdentityRequest,
    responseType: Admin_IdentityService_pb.DeleteIdentityResponse,
    requestSerialize: serialize_DeleteIdentityRequest,
    requestDeserialize: deserialize_DeleteIdentityRequest,
    responseSerialize: serialize_DeleteIdentityResponse,
    responseDeserialize: deserialize_DeleteIdentityResponse,
  },
  getIdentity: {
    path: '/IdentityService/GetIdentity',
    requestStream: false,
    responseStream: false,
    requestType: Admin_IdentityService_pb.GetIdentityRequest,
    responseType: Admin_IdentityService_pb.GetIdentityResponse,
    requestSerialize: serialize_GetIdentityRequest,
    requestDeserialize: deserialize_GetIdentityRequest,
    responseSerialize: serialize_GetIdentityResponse,
    responseDeserialize: deserialize_GetIdentityResponse,
  },
};

exports.IdentityServiceClient = grpc.makeGenericClientConstructor(IdentityServiceService);
