// works in powershell

grpc_tools_node_protoc `
	-I..\Proto `
    --js_out=import_style=commonjs,binary:.\proto\ `
    --grpc_out=.\proto\ `
    --plugin=protoc-gen-grpc=.\node_modules\grpc-tools\bin\grpc_node_plugin.exe `
    ..\Proto\Admin\IdentityService.proto


grpc_tools_node_protoc `
	-I..\Proto `
    --js_out=import_style=commonjs,binary:.\proto\ `
    --ts_out=.\proto\ `
    --grpc_out=.\proto\ `
    --plugin=protoc-gen-ts='node ./node_modules/.bin/protoc-gen-ts' `
    ..\Proto\Admin\IdentityService.proto

// dunno

grpc_tools_node_protoc \
	-I../Proto \
    --js_out=import_style=commonjs,binary:./proto/ \
    --grpc_out=./proto/ \
    --plugin=protoc-gen-grpc=`which grpc_tools_node_protoc_plugin` \
    ../Proto/Admin/IdentityService.proto