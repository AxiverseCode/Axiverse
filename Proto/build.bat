grpc_tools_node_protoc \
    --js_out=import_style=commonjs,binary:../node/static_codegen/
    --grpc_out=../node/static_codegen
    --plugin=protoc-gen-grpc=`which grpc_tools_node_protoc_plugin`
    helloworld.proto



..\Source\packages\Grpc.Tools.1.10.0\tools\windows_x86\protoc.exe ^
	-I. ^
	--csharp_out ..\Source\Services\Axiverse.Services.Proto\ ^
	--grpc_out ..\Source\Services\Axiverse.Services.Proto\ ^
	.\Admin\AliasService.proto ^
	.\Admin\IdentityService.proto ^
	.\Infrastructure\MetricService.proto ^
	.\AssetService.proto ^
	.\ChatService.proto ^
	.\EntityService.proto ^
	.\MarketService.proto ^
	.\ModelService.proto ^
	.\ResourceService.proto ^
	--plugin=protoc-gen-grpc=..\Source\packages\Grpc.Tools.1.10.0\tools\windows_x86\grpc_csharp_plugin.exe