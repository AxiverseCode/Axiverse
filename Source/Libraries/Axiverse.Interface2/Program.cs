using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;

using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace Axiverse.Interface2
{
    using Vector3 = SharpDX.Vector3;

    class Program
    {
        public static Stopwatch watch = new Stopwatch();
        static int frame = 0;

        static void Main(string[] args)
        {
            using (var form = new RenderForm())
            using (var device = new Device(form))
            {
                Mesh mesh = Mesh.CreateCube(device);
                Shader shader = new Shader(device, "../../Shader.hlsl", "VS", "PS", Mesh.ColoredTexturedVertex.Elements);
                Buffer11 buffer = device.CreateBuffer<Matrix>();
                ShaderResourceView texture = Texture.CreateTextureFromBitmap(device, "../../Texture.png");

                watch.Start();
                RenderLoop.Run(form, () =>
                {
                    device.Start();
                    //clear color
                    device.Clear(Color.CornflowerBlue);

                    //Set matrices
                    float ratio = (float)form.ClientRectangle.Width / (float)form.ClientRectangle.Height;
                    Matrix projection = Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 1000);
                    Matrix view = Matrix.LookAtLH(new Vector3(0, 10, -50), new Vector3(), Vector3.UnitY);
                    Matrix world = Matrix.RotationY(watch.ElapsedMilliseconds / 1000.0F);
                    Matrix worldViewProjection = world * view * projection;

                    //update constant buffer
                    device.UpdateData<Matrix>(buffer, worldViewProjection);

                    //pass constant buffer to shader
                    device.NativeDeviceContext.VertexShader.SetConstantBuffer(0, buffer);
                    device.NativeDeviceContext.PixelShader.SetShaderResource(0, texture);

                    //apply shader
                    shader.Apply();

                    //draw mesh
                    mesh.Draw();

                    device.Canvas.Begin();
                    frame++;
                    var seconds = watch.ElapsedMilliseconds / 1000f;
                    device.Canvas.DrawString(1 / (seconds / frame) + " fps ", 10, 10);
                    device.Canvas.End();

                    device.Present();
                });
            }
        }
    }
}
