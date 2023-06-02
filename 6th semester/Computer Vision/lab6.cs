using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CompVision
{
    public class Camera
    {
        // Those vectors are directions pointing outwards from the camera to define how it rotated.
        private Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;

        // Rotation around the X axis (radians)
        private float _pitch;

        // Rotation around the Y axis (radians)
        private float _yaw = -MathHelper.PiOver2; // Without this, you would be started rotated 90 degrees right.

        // The field of view of the camera (radians)
        private float _fov = MathHelper.PiOver2;

        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }

        // The position of the camera
        public Vector3 Position { get; set; }

        // This is simply the aspect ratio of the viewport, used for the projection matrix.
        public float AspectRatio { private get; set; }

        public Vector3 Front => _front;

        public Vector3 Up => _up;

        public Vector3 Right => _right;

        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        // The field of view (FOV) is the vertical angle of the camera view.
        // This has been discussed more in depth in a previous tutorial,
        // but in this tutorial, you have also learned how we can use this to simulate a zoom feature.
        // We convert from degrees to radians as soon as the property is set to improve performance.
        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 90f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        // Get the projection matrix using the same method we have used up until this point
        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
        }

        // This function is going to update the direction vertices using some of the math learned in the web tutorials.
        private void UpdateVectors()
        {
            // First, the front matrix is calculated using some basic trigonometry.
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
            _front = Vector3.Normalize(_front);

            // Calculate both the right and the up vector using cross product.
            // Note that we are calculating the right from the global up; this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }
    }

    public class Shader
    {
        public int Handle;
        private readonly Dictionary<string, int> _uniformLocations;

        public Shader(string vertexPath, string fragmentPath)
        {
            // read shaders
            string VertexShaderSource = File.ReadAllText(vertexPath);
            string FragmentShaderSource = File.ReadAllText(fragmentPath);


            // generate our shaders, and bind the source code to the shaders.
            int VertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShader, VertexShaderSource);

            int FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShader, FragmentShaderSource);

            // compile shaders 
            GL.CompileShader(VertexShader);

            GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(VertexShader);
                Console.WriteLine(infoLog);
            }

            GL.CompileShader(FragmentShader);

            GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success1);
            if (success1 == 0)
            {
                string infoLog = GL.GetShaderInfoLog(FragmentShader);
                Console.WriteLine(infoLog);
            }

            Handle = GL.CreateProgram();

            GL.AttachShader(Handle, VertexShader);
            GL.AttachShader(Handle, FragmentShader);

            GL.LinkProgram(Handle);

            GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success2);
            if (success2 == 0)
            {
                string infoLog = GL.GetProgramInfoLog(Handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(Handle, VertexShader);
            GL.DetachShader(Handle, FragmentShader);
            GL.DeleteShader(FragmentShader);
            GL.DeleteShader(VertexShader);

            // get the number of active uniforms in the shader.
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            // allocate the dictionary to hold the locations.
            _uniformLocations = new Dictionary<string, int>();
            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);
                // get the location,
                var location = GL.GetUniformLocation(Handle, key);
                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }

        public void SetVector3(string name, Vector3 data)
        {
            GL.UseProgram(Handle);
            GL.Uniform3(_uniformLocations[name], data);
        }

        public void SetFloat(string name, float data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        public void SetInt(string name, int data)
        {
            GL.UseProgram(Handle);
            GL.Uniform1(_uniformLocations[name], data);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            if (disposedValue == false)
            {
                Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    class Game : GameWindow
    {
        //private readonly float[] _vertices =
        //{
        //    -0.5f, -0.5f, 0.0f, // Bottom-left vertex
        //     0.5f, -0.5f, 0.0f, // Bottom-right vertex
        //     0.0f,  0.5f, 0.0f  // Top vertex
        //};//triangle

        //float[] _vertices = {
        //     0.5f,  0.5f, 0.0f, 0.5f, 0.5f, 1.0f,  // top right
        //     0.5f, -0.5f, 0.0f, 0.5f, 0.5f, 1.0f,  // bottom right
        //    -0.5f, -0.5f, 0.0f, 0.5f, 0.5f, 1.0f,  // bottom left
        //    -0.5f,  0.5f, 0.0f, 0.5f, 0.5f, 1.0f   // top left
        //};//rect colored

        static Vector3 p1 = new Vector3(-0.33f, -0.5f, -0.33f);
        static Vector3 p2 = new Vector3(-0.33f, -0.5f, 0.33f);
        static Vector3 p3 = new Vector3(0.33f, -0.5f, -0.33f);
        static Vector3 n13 = GetNormal(p1, p2, p3);
        static Vector3 p4 = new Vector3(0.33f, -0.5f, -0.33f);
        static Vector3 p5 = new Vector3(0f, 0.33f, 0f);
        static Vector3 p6 = new Vector3(-0.33f, -0.5f, 0.33f);
        static Vector3 n46 = GetNormal(p4, p5, p6);
        static Vector3 p7 = new Vector3(-0.33f, -0.5f, 0.33f);
        static Vector3 p8 = new Vector3(0f, 0.33f, 0f);
        static Vector3 p9 = new Vector3(-0.33f, -0.5f, -0.33f);
        static Vector3 n79= GetNormal(p7, p8, p9);
        static Vector3 p10 = new Vector3(-0.33f, -0.5f, -0.33f);
        static Vector3 p11 = new Vector3(0f, 0.33f, 0f);
        static Vector3 p12 = new Vector3(0.33f, -0.5f, -0.33f);
        static Vector3 n1012= GetNormal(p10, p11, p12);



        //private float[] _vertices = GenerateSphereVertices(1f, 20);
        private float[] _vertices =
        {
             // Position          Normal
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Front face
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
        
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Back face
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
        
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, // Left face
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
        
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, // Right face
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
        
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Bottom face
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
        
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Top face
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
        };

        //readonly float[] _vertices2 = Cone(); //uncomment this line and hide next block for cone draw
        readonly float[] _vertices2 =
        {
             // Position          Normal
             p1.X, p1.Y, p1.Z, n13.X, n13.Y, n13.Z, // botom face
             p2.X, p2.Y, p2.Z, n13.X, n13.Y, n13.Z,
             p3.X, p3.Y, p3.Z, n13.X, n13.Y, n13.Z, 
             p4.X, p4.Y, p4.Z, n46.X, n46.Y, n46.Z, // front face
             p5.X, p5.Y, p5.Z, n46.X, n46.Y, n46.Z,
             p6.X, p6.Y, p6.Z, n46.X, n46.Y, n46.Z, 
             p7.X, p7.Y, p7.Z, n79.X, n79.Y, n79.Z, // left face
             p8.X, p8.Y, p8.Z, n79.X, n79.Y, n79.Z,
             p9.X, p9.Y, p9.Z, n79.X, n79.Y, n79.Z, 
             p10.X, p10.Y, p10.Z, n1012.X, n1012.Y, n1012.Z, // right face
             p11.X, p11.Y, p11.Z, n1012.X, n1012.Y, n1012.Z,
             p12.X, p12.Y, p12.Z, n1012.X, n1012.Y, n1012.Z, 
        };

        readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);

        uint[] _indices = {
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        const float _timeFreq = 10f;
        const float _cameraSpeed = 1.5f;
        const float _sensitivity = 0.2f;

        double _time;
        Matrix4 _view;
        Matrix4 _projection;
        int _vertexBufferObject;
        int _vertexBufferObject2;
        int _vaoModel;
        int _vaoPyramid;
        int _vaoLamp;
        Shader _lampShader;
        Shader _lightingShader;
        Shader _pyramidShader;
        Camera _camera;
        bool _firstMove = true;
        Vector2 _lastPos;
        Vector3 _lightColor = new Vector3(1f, 0.5f, 0.2f);

        public Game(int width, int height, string title) : 
            base(GameWindowSettings.Default, new NativeWindowSettings() 
            { Size = (width, height), Title = title }) { }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState input = KeyboardState;

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * _cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * _cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * _cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * _cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * _cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * _cameraSpeed * (float)e.Time; // Down
            }

            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _camera.Yaw += deltaX * _sensitivity;
                _camera.Pitch -= deltaY * _sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // clear window with spec color
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            // enable depth testing
            GL.Enable(EnableCap.DepthTest);
            // create a buffer.
            _vertexBufferObject = GL.GenBuffer();
            // bind the buffer.
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            // upload the vertices to the buffer.
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // shader.vert and shader.frag contain the actual shader code.
            _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

            // generate and bind a VAO
            _vaoModel = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel);

            var positionLocation = _lightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            var normalLocation = _lightingShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);

            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            _vaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(_vaoLamp);

            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
            // We make the mouse cursor invisible and captured so we can have proper FPS-camera movement.
            CursorState = CursorState.Grabbed;

            // Pyramid binding
            _vertexBufferObject2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject2);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices2.Length * sizeof(float), _vertices2, BufferUsageHint.StaticDraw);
            _pyramidShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            _vaoPyramid = GL.GenVertexArray();
            GL.BindVertexArray(_vaoPyramid);

            var positionLocation2 = _lightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation2);
            GL.VertexAttribPointer(positionLocation2, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            var normalLocation2 = _lightingShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation2);
            GL.VertexAttribPointer(normalLocation2, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _time += _timeFreq * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // Bind the VAO
            GL.BindVertexArray(_vaoModel);
            //bind shader
            _lightingShader.Use();

            _lightingShader.SetMatrix4("model", Matrix4.Identity);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            _lightingShader.SetVector3("viewPos", _camera.Position);

            // set the material values of the cube, the material struct uniform
            _lightingShader.SetVector3("material.ambient", new Vector3(1.0f, 0.5f, 0.31f));
            _lightingShader.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
            _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader.SetFloat("material.shininess", 32.0f);

            //// This is where we change the lights color over time using the sin function
            //Vector3 lightColor;
            //float time = DateTime.Now.Second + DateTime.Now.Millisecond / 1000f;
            //lightColor.X = (MathF.Sin(time * 2.0f) + 1) / 2f;
            //lightColor.Y = (MathF.Sin(time * 0.7f) + 1) / 2f;
            //lightColor.Z = (MathF.Sin(time * 1.3f) + 1) / 2f;

            // The ambient light is less intensive than the diffuse light in order to make it less dominant
            Vector3 ambientColor = _lightColor * new Vector3(0.2f);
            Vector3 diffuseColor = _lightColor * new Vector3(0.5f);

            _lightingShader.SetVector3("light.position", _lightPos);
            _lightingShader.SetVector3("light.ambient", ambientColor);
            _lightingShader.SetVector3("light.diffuse", diffuseColor);
            _lightingShader.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length); // 2(traingles)x3(vertices)x6(sides)

            GL.BindVertexArray(_vaoLamp);
            _lampShader.Use();
            Matrix4 lampMatrix = Matrix4.Identity;
            lampMatrix *= Matrix4.CreateScale(0.2f);
            lampMatrix *= Matrix4.CreateTranslation(_lightPos);
            _lampShader.SetMatrix4("model", lampMatrix);
            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);


            Vector3 _lightColor2 = new Vector3(0.5f, 0.7f, 0.2f);
            Vector3 ambientColor2 = _lightColor2 * new Vector3(0.2f);
            Vector3 diffuseColor2 = _lightColor2 * new Vector3(0.5f);
            GL.BindVertexArray(_vaoPyramid);
            _pyramidShader.Use();
            _pyramidShader.SetMatrix4("model", Matrix4.Identity * Matrix4.CreateTranslation(0f, 2f, 0f));
            _pyramidShader.SetMatrix4("view", _camera.GetViewMatrix());
            _pyramidShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _pyramidShader.SetVector3("viewPos", _camera.Position);
            _pyramidShader.SetVector3("material.ambient", new Vector3(1.0f, 0.5f, 0.31f));
            _pyramidShader.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
            _pyramidShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _pyramidShader.SetFloat("material.shininess", 32.0f);
            _pyramidShader.SetVector3("light.position", _lightPos);
            _pyramidShader.SetVector3("light.ambient", ambientColor2);
            _pyramidShader.SetVector3("light.diffuse", diffuseColor2);
            _pyramidShader.SetVector3("light.specular", new Vector3(1.0f, 1.0f, 1.0f));
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices2.Length); // 2(traingles)x3(vertices)x6(sides)
            //  432 verts for cone with div==10

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            // update the aspect ratio
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }

        protected override void OnUnload()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteBuffer(_vertexBufferObject2);
            GL.DeleteVertexArray(_vaoLamp);
            GL.DeleteVertexArray(_vaoModel);
            GL.DeleteVertexArray(_vaoPyramid);
            GL.DeleteProgram(_lightingShader.Handle);
            GL.DeleteProgram(_pyramidShader.Handle);
            GL.DeleteProgram(_lampShader.Handle);
            _lightingShader.Dispose();
            _pyramidShader.Dispose();
            _lampShader.Dispose();

            base.OnUnload();
        }

        static Vector3 GetNormal(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 res = new Vector3();
            var _p1 = new Vector3d(p1.X, p1.Y, p1.Z);
            var _p2 = new Vector3d(p2.X, p2.Y, p2.Z);
            var _p3 = new Vector3d(p3.X, p3.Y, p3.Z);
            Vector3d a = _p2 - _p1;
            Vector3d b = _p3 - _p1;
            var temp = Vector3d.Cross(a, b);
            temp.Normalize();
            res.X = Convert.ToSingle(temp.X);
            res.Y = Convert.ToSingle(temp.Y);
            res.Z = Convert.ToSingle(temp.Z);
            return res;
        }

        static float[] GenerateSphereVertices(float radius, int subdivisions)
        {
            int sectorCount = 48;
            int stackCount = 24;
            int counter = 0;
            float x, y, z, xy;                              // vertex position
            float nx, ny, nz, lengthInv = 1.0f / radius;    // vertex normal

            float sectorStep = 2 * MathF.PI / sectorCount;
            float stackStep = MathF.PI / stackCount;
            float sectorAngle, stackAngle;

            float[] vertices = new float[(sectorCount+1) * (stackCount+1) * 6];
            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = MathF.PI / 2 - i * stackStep;        // starting from pi/2 to -pi/2
                xy = radius * MathF.Cos(stackAngle);             // r * cos(u)
                z = radius * MathF.Sin(stackAngle);              // r * sin(u)

                // add (sectorCount+1) vertices per stack
                // first and last vertices have same position and normal, but different tex coords
                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;           // starting from 0 to 2pi

                    // vertex position (x, y, z)
                    x = xy * MathF.Cos(sectorAngle);             // r * cos(u) * cos(v)
                    y = xy * MathF.Sin(sectorAngle);             // r * cos(u) * sin(v)
                    int k = j % 2 == 0 ? 1 : 0;
                    vertices[counter + k*3] = x;
                    vertices[counter + k * 3 + 1] = y;
                    vertices[counter + k * 3 + 2] = z;
                    counter += 3;
                    // normalized vertex normal (nx, ny, nz)
                    nx = x * lengthInv;
                    ny = y * lengthInv;
                    nz = z * lengthInv;
                    vertices[counter++] = nx;
                    vertices[counter++] = ny;
                    vertices[counter++] = nz;
                }
            }
            return vertices;
        }
		
        static float[] Cone()
        {
            int offset = 9;
            int div = 10;
            int size = (360 / div + 2) * offset * 4;
            int counter = 0;
            Vector3 top = new Vector3(0f, 0.5f, 0f);
            Vector3 bottom = new Vector3(0f, 0f, 0f);
            float[] vert = new float[size];
            float[] b, c;

            b = new float[3]; c = new float[3];
            b[1] = c[1] = 0f;

            for (float angle = 0f; angle <= 360f; angle += div)
            {
                c[0] = b[0];
                c[2] = b[2];

                b[0] = MathF.Cos(angle * MathF.PI / 180.0f);
                b[2] = MathF.Sin(angle * MathF.PI / 180.0f);
                int i = counter * offset * 4;
                if (angle != 0)
                {
                    Vector3 normal = GetNormal(top,
                        new Vector3(c[0], c[1], c[2]),
                        new Vector3(b[0], b[1], b[2]));
                    vert[i] = top.X;
                    vert[i+1] = top.Y;
                    vert[i+2] = top.Z;
                    vert[i+3] = normal.X;
                    vert[i+4] = normal.Y;
                    vert[i+5] = normal.Z;
                    vert[i+6] =     (float)c[0];
                    vert[i+7] =   (float)c[1];
                    vert[i+8] =   (float)c[2];
                    vert[i+9] =   normal.X;
                    vert[i+10] =   normal.Y;
                    vert[i+11] =   normal.Z;
                    vert[i+12] =   (float)b[0];
                    vert[i+13] = (float)b[1];
                    vert[i+14] = (float)b[2];
                    vert[i+15] = normal.X;
                    vert[i+16] = normal.Y;
                    vert[i+17] = normal.Z;

                    normal = GetNormal(bottom,
                        new Vector3(c[0], c[1], c[2]),
                        new Vector3(b[0], b[1], b[2]));
                    vert[i+2*offset] =   (float)b[0];
                    vert[i+2*offset+1] = (float)b[1];
                    vert[i+2*offset+2] = (float)b[2];
                    vert[i+2*offset+3] = normal.X;
                    vert[i+2*offset+4] = normal.Y;
                    vert[i+2*offset+5] = normal.Z;
                    vert[i+2*offset+6] =   bottom.X;
                    vert[i+2*offset+7] = bottom.Y;
                    vert[i+2*offset+8] = bottom.Z;
                    vert[i+2*offset+9] = normal.X;
                    vert[i+2*offset+10] = normal.Y;
                    vert[i+2*offset+11] = normal.Z;
                    vert[i+2*offset+12] =   (float)c[0];
                    vert[i+2*offset+13] = (float)c[1];
                    vert[i+2*offset+14] = (float)c[2];
                    vert[i+2*offset+15] = normal.X;
                    vert[i+2*offset+16] = normal.Y;
                    vert[i+2*offset+17] = normal.Z;
                    counter++;
                }
            }
            return vert;
        }
    }

        static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (Game game = new Game(800, 600, "Lab6"))
            {
                game.Run();
            }
        }
    }
}