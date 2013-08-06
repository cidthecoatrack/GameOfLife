using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using GameOfLife.Core;
using SharpGL;
using SharpGL.SceneGraph;

namespace GameOfLife.UI
{
    public partial class MainWindow : Window
    {
        private const Int32 TickDelay = 250;
        private const Int32 CameraMoveSensitivity = 1;
        private const Int32 CameraRotationSensitivity = 240;

        private Int32 gridOffset { get { return edgeSize / 2; } }

        private Timer timer;
        private OpenGL gl;
        private Game game;
        private Camera camera;
        private Int32 edgeSize;

        public Int32 EdgeSize 
        {
            get { return edgeSize; }
            set
            {
                if (value >= 1 && value <= 100)
                {
                    edgeSize = value;
                    game = new Game(value);
                    camera.SetZPosition(value + gridOffset);
                }
            }
        }

        public MainWindow()
        {
            edgeSize = 15;

            InitializeComponent();
            timer = new Timer();
            timer.Interval = TickDelay;
            timer.Elapsed += (s, a) => game.Tick();
            camera = new Camera();

            game = new Game(edgeSize);

            DataContext = this;
        }

        private void StartSimulation()
        {
            game = new Game(EdgeSize);
            PauseOrResumeSimulation();
        }

        private void openGLControl_OpenGLDraw(Object sender, OpenGLEventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            DrawLiveCells();
            SetCameraView();
        }

        private void DrawLiveCells()
        {
            var cells = game.Grid.Cells;

            for (var x = 0; x < EdgeSize; x++)
                for (var y = 0; y < EdgeSize; y++)
                    for (var z = 0; z < EdgeSize; z++)
                        if (cells[x][y][z])
                            DrawCube(x - gridOffset, y - gridOffset, z - gridOffset);
        }

        private void DrawCube(Int32 x, Int32 y, Int32 z)
        {
            DrawSide(x,     y,     z, 
                     x + 1, y,     z,
                     x + 1, y + 1, z,
                     x,     y + 1, z);

            DrawSide(x + 1, y,     z,
                     x + 1, y + 1, z,
                     x + 1, y + 1, z - 1,
                     x + 1, y,     z - 1);

            DrawSide(x,     y + 1, z,
                     x,     y + 1, z - 1,
                     x,     y,     z - 1,
                     x,     y,     z);

            DrawSide(x,     y + 1, z - 1,
                     x + 1, y + 1, z - 1,
                     x + 1, y + 1, z,
                     x,     y + 1, z);

            DrawSide(x,     y,     z - 1,
                     x + 1, y,     z - 1,
                     x + 1, y,     z,
                     x,     y,     z);

            DrawSide(x + 1, y,     z - 1,
                     x + 1, y + 1, z - 1,
                     x,     y + 1, z - 1,
                     x,     y,     z - 1);
        }

        private void DrawSide(Int32 v1x, Int32 v1y, Int32 v1z, Int32 v2x, Int32 v2y, Int32 v2z, 
                              Int32 v3x, Int32 v3y, Int32 v3z, Int32 v4x, Int32 v4y, Int32 v4z)
        {
            byte red = 31;
            byte green = 61;
            byte blue = 12;

            gl.Color(red, green, blue);
            gl.Begin(OpenGL.GL_POLYGON);
            gl.Vertex(v1x, v1y, v1z);
            gl.Vertex(v2x, v2y, v2z);
            gl.Vertex(v3x, v3y, v3z);
            gl.Vertex(v4x, v4y, v4z);
            gl.End();

            red = 255;
            green = 255;
            blue = 255;

            gl.Color(red, green, blue);
            gl.LineWidth(3.0f);
            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Vertex(v1x, v1y, v1z);
            gl.Vertex(v2x, v2y, v2z);
            gl.Vertex(v3x, v3y, v3z);
            gl.Vertex(v4x, v4y, v4z);
            gl.Vertex(v1x, v1y, v1z);
            gl.End();
        }

        private void SetCameraView()
        {
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(60.0f, (Double)Width / (Double)Height, 0.01, 1000.0);
            gl.LookAt(
                camera.Position.X,
                camera.Position.Y,
                camera.Position.Z,
                camera.Target.X,
                camera.Target.Y,
                camera.Target.Z,
                camera.Up.X,
                camera.Up.Y,
                camera.Up.Z);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void openGLControl_OpenGLInitialized(Object sender, OpenGLEventArgs args)
        {
            camera.SetZPosition(EdgeSize + gridOffset);
            gl = openGLControl.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
        }

        private void openGLControl_Resized(Object sender, OpenGLEventArgs args)
        {
            SetCameraView();
        }

        private void mainWindow_KeyDown(Object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
                StartSimulation();

            if (Keyboard.IsKeyDown(Key.Space))
                PauseOrResumeSimulation();

            if (Keyboard.IsKeyDown(Key.A))
                camera.Rotate(new Vector3D(CameraRotationSensitivity, 0, 0));

            if (Keyboard.IsKeyDown(Key.D))
                camera.Rotate(new Vector3D(-CameraRotationSensitivity, 0, 0));

            if (Keyboard.IsKeyDown(Key.W))
                camera.Rotate(new Vector3D(0, CameraRotationSensitivity, 0));

            if (Keyboard.IsKeyDown(Key.S))
                camera.Rotate(new Vector3D(0, -CameraRotationSensitivity, 0));
        }

        private void mainWindow_MouseWheel(Object sender, MouseWheelEventArgs e)
        {
            var amountToMove = e.Delta / 120 * CameraMoveSensitivity;
            camera.MoveForward(amountToMove);
        }

        private void RunButton_Click(Object sender, RoutedEventArgs e)
        {
            PauseOrResumeSimulation();
        }

        private void PauseOrResumeSimulation()
        {
            if (timer.Enabled)
            {
                timer.Stop();
                RunButton.Content = "Start";
            }
            else
            {
                timer.Start();
                RunButton.Content = "Pause";
            }
        }

        private void Reset_Click(Object sender, RoutedEventArgs e)
        {
            timer.Stop();
            RunButton.Content = "Start";
            game = new Game(EdgeSize);
            camera.SetZPosition(EdgeSize + gridOffset);
        }
    }
}