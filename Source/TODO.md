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