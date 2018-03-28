..\packages\Grpc.Tools.1.10.0\tools\windows_x86\protoc.exe ^
	-I..\..\Proto ^
	--csharp_out . ^
	--grpc_out . ^
	..\..\Proto\Admin\AliasService.proto ^
	..\..\Proto\Admin\IdentityService.proto ^
	..\..\Proto\Interface\MetricService.proto ^
	..\..\Proto\AssetService.proto ^
	..\..\Proto\EntityService.proto ^
	..\..\Proto\MarketService.proto ^
	..\..\Proto\ModelService.proto ^
	..\..\Proto\ResourceService.proto ^
	--plugin=protoc-gen-grpc=..\packages\Grpc.Tools.1.10.0\tools\windows_x86\grpc_csharp_plugin.exe