### 12/6

- Have server be the authority.
- User can query what they have permission to access and control.
- Client can then change context and modify what it has permissions to control.

### 7/9

- Serialization
	- Lex/parse - JSON for most
	- Write support for files
- Input routing
- Gui implementation
- Flush out math library
- Screen space selection






### 7/5

- Control a ship
- Have other ships move randomly
- Integrate some game mechanics
- Better input routing.
- Tying entity state to GUI

- Resource management and loading
- Mipmap generation.

### Pre 7/5

Binding
Producers
Module

Model - uses producers to advance and inject to access services
Universe - which advances entities based on models

EntityService - which gives access to a universe. Can be set up and then controlled and utilized.
	We should be able to create different instances of the EntityService.

After this point we should be able to create our first prototype entities and models. And then
build the systems around them to support their interconnection.

- Physics
- Weapon Systems
- Control Systems
- Identity and Login
- Remote client access.

At this point we should be able to fly around in a world where other things are flying around as
well.

Next steps:

Resource management - programmatic or automatic from file. Uri generated.
Compositor
- Setup frame and present frame
Renderer
- ForwardRenderer specialization
RenderStage
- Support sorting and drawers.
ModelDrawer
- Performs draw calls for the model objects.
- Sets the necessary fields in the effect.

Matrix4
- Implement a lot of the stuff.

Simplify engine
- Separate model generation and store in resource manager.
- Automatically compute the transform from the components.
- Put attribute into components.
- Render using renderers.

We have a functional engine!

Camera controls and inputs.

Let's do some game stuff. ^_^

