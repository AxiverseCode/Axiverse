echo %cd%

echo %1
echo %2
%1\packages\Grpc.Tools.1.15.0\tools\windows_x86\protoc.exe ^
	-I%1\Proto ^
	--csharp_out %2 ^
	--grpc_out %2 ^
	%1\Proto\Admin\AliasService.proto ^
	%1\Proto\Admin\IdentityService.proto ^
	%1\Proto\Infrastructure\MetricService.proto ^
	%1\Proto\AssetService.proto ^
	%1\Proto\EntityService.proto ^
	%1\Proto\MarketService.proto ^
	%1\Proto\ModelService.proto ^
	%1\Proto\ResourceService.proto ^
	--plugin=protoc-gen-grpc=%1\packages\Grpc.Tools.1.15.0\tools\windows_x86\grpc_csharp_plugin.exe