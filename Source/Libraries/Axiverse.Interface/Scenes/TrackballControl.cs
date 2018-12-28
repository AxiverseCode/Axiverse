using Axiverse.Interface.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    public class TrackballControl
    {
        // https://github.com/mrdoob/three.js/blob/master/examples/js/controls/TrackballControls.js

        public enum State
        {
            None,
            Rotate,
            Zoom,
            Pan,
            TouchRotate,
            TouchZoomPan,
        }

        /// <summary>
        /// Gets or sets whether the camera controller is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the dimensions of the screen.
        /// </summary>
        public Rectangle Screen { get; set; }

        /// <summary>
        /// Gets or sets the pan speed;
        /// </summary>
        public float RotateSpeed { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the zoom speed;
        /// </summary>
        public float ZoomSpeed { get; set; } = 1.2f / 180;

        /// <summary>
        /// Gets or sets the pan speed;
        /// </summary>
        public float PanSpeed { get; set; } = 0.3f;

        /// <summary>
        /// Gets or sets whether rotations are enabled.
        /// </summary>
        public bool RotateEnabled { get; set; }

        /// <summary>
        /// Gets or sets whether zoom is enabled.
        /// </summary>
        public bool ZoomEnabled { get; set; }

        /// <summary>
        /// Gets or sets whether panning is enabled.
        /// </summary>
        public bool PanEnabled { get; set; }

        /// <summary>
        /// Gets or sets whether momentum moving is off.
        /// </summary>
        public bool StaticMoving { get; set; } = false;

        /// <summary>
        /// Gets or sets the dynamic dampening factor for momentum moving;
        /// </summary>
        public float DynamicDampeningFactor { get; set; } = 0.2f;

        /// <summary>
        /// The minimum distance for zooming.
        /// </summary>
        public float MinDistance { get; set; } = 1;

        /// <summary>
        /// The maximum distance for zooming.
        /// </summary>
        public float MaxDistance { get; set; } = 1000f;


        private Vector3 target;
        private float epsilon = 0.000001f;
        private Vector3 lastPosition;
        private State state = State.None;
        private State previousState = State.None;
        private Vector3 eye; // eye relative to target
        private Vector2 movePrevious;
        private Vector2 moveCurrent;
        private Vector3 lastAxis;
        private float lastAngle;
        public Quaternion lastRotation;
        public Vector2 lastRotationVector;
        private Vector2 zoomStart;
        private Vector2 zoomEnd;
        private float touchZoomDistanceStart = 0;
        private float touchZoomDistanceEnd = 0;
        private Vector2 panStart;
        private Vector2 panEnd;

        private Vector3 objectPosition;
        private Vector3 objectUpDirection;

        public Vector3 CameraPosition
        {
            get => objectPosition;
            set => objectPosition = value;
        }

        public Vector3 Target
        {
            get => target;
            set => target = value;
        }

        public Vector3 Up
        {
            get => objectUpDirection;
            set => objectUpDirection = value;
        }

        void HandlResize(Rectangle screen)
        {
            this.Screen = screen;
        }

        Vector2 GetMouseOnScreen(Vector2 page) =>
            new Vector2(
                (page.X - Screen.Left) / Screen.Width,
                (page.Y - Screen.Top) / Screen.Height);

        Vector2 GetMouseOnCircle(Vector2 page) =>
            new Vector2(
                (page.X - Screen.Width / 2 - Screen.Left) / (Screen.Width / 2),
                (Screen.Height + 2 * (Screen.Top - page.Y)) / Screen.Width);

        void RotateCamera()
        {
            //Vector3 moveDirection = new Vector3(moveCurrent.X - movePrevious.X, moveCurrent.Y - movePrevious.Y, 0);
            //float angle = moveDirection.Length();
            float angle = new Vector2(moveCurrent.X - movePrevious.X, moveCurrent.Y - movePrevious.Y).Length();
            System.Diagnostics.Debug.WriteLine(moveCurrent);

            if (angle != 0)
            {
                eye = objectPosition - target;

                Vector3 eyeDirection = eye.Normal();
                Vector3 objectUp = this.objectUpDirection.Normal();
                Vector3 objectSideways = (objectUp % eyeDirection).Normal();

                float deltaY = moveCurrent.Y - movePrevious.Y;
                float deltaX = moveCurrent.X - movePrevious.X;

                objectUp.SetLength(deltaY);
                objectSideways.SetLength(deltaX);

                Vector3 moveDirection = objectUp + objectSideways;
                Vector3 axis = (moveDirection % eyeDirection).Normal();

                angle *= RotateSpeed;
                Quaternion quaternion = Quaternion.FromAxisAngle(axis, angle);

                eye = quaternion.Transform(eye);
                this.objectUpDirection = quaternion.Transform(this.objectUpDirection);

                lastRotation = quaternion;
                lastRotationVector = new Vector2(deltaX, deltaY);
                lastAxis = axis;
                lastAngle = angle;
            }
            else if (!StaticMoving && lastAngle != 0)
            {
                lastAngle *= Functions.Sqrt(1 - DynamicDampeningFactor);
                eye = this.objectPosition - target;
                Quaternion quaternion = Quaternion.FromAxisAngle(lastAxis, lastAngle);
                eye = quaternion.Transform(eye);
                lastRotation = quaternion;
                this.objectUpDirection = quaternion.Transform(this.objectUpDirection);
            }
            movePrevious = moveCurrent;
        }

        void ZoomCamera()
        {
            if (state == State.TouchZoomPan)
            {
                float factor = touchZoomDistanceStart / touchZoomDistanceEnd;
                touchZoomDistanceStart = touchZoomDistanceEnd;
                eye *= factor;
            }
            else
            {
                float factor = 1 + (zoomEnd.Y - zoomStart.Y) * ZoomSpeed;

                if (factor != 1 && factor > 0)
                {
                    eye *= factor;
                }

                if (StaticMoving)
                {
                    zoomStart = zoomEnd;
                }
                else
                {
                    zoomStart.Y += (zoomEnd.Y - zoomStart.Y) * DynamicDampeningFactor;
                }
            }
        }


        void PanCamera()
        {
            Vector2 mouseChange = panEnd - panStart;

            if (mouseChange != Vector2.Zero)
            {
                mouseChange *= eye.Length() * PanSpeed;
                Vector3 pan = (eye % objectUpDirection).OfLength(mouseChange.X);
                pan += objectUpDirection.OfLength(mouseChange.Y);

                objectPosition += pan;
                target += pan;

                if (StaticMoving)
                {
                    panStart = panEnd;
                }
                else
                {
                    panStart += (panEnd - panStart) * DynamicDampeningFactor;
                }
            }
        }

        void CheckDistances()
        {
            if (ZoomEnabled || PanEnabled)
            {
                if (eye.LengthSquared() > MaxDistance * MaxDistance)
                {
                    objectPosition = target + eye.OfLength(MaxDistance);
                    zoomStart = zoomEnd;
                }

                if (eye.LengthSquared() < MinDistance * MinDistance)
                {
                    objectPosition = target + eye.OfLength(MinDistance);
                    zoomStart = zoomEnd;
                }
            }
        }

        public void Update()
        {
            eye = objectPosition - target;
            lastRotation = Quaternion.Identity;

            if (RotateEnabled)
            {
                RotateCamera();
            }

            if (ZoomEnabled)
            {
                ZoomCamera();
            }

            if (PanEnabled)
            {
                PanCamera();
            }

            objectPosition = target + eye;
            CheckDistances();
            // object.lookAt(target);

            if (lastPosition.DistanceToSquared(objectPosition) > epsilon)
            {
                // dispatchevent (changed)
                lastPosition = objectPosition;
            }
        }

        public void Reset()
        {
            state = State.None;
            previousState = State.None;
        }

        public void OnKeyDown(char key)
        {
            if (!Enabled)
            {
                return;
            }

            // removeEventListener(keydown)

            previousState = state;
            if (state != State.None)
            {
                return;
            }

            if (key == 'r' && RotateEnabled)
            {
                state = State.Rotate;
            }
            else if (key == 'z' && ZoomEnabled)
            {
                state = State.Zoom;
            }
            else if (key == 'p' && PanEnabled)
            {
                state = State.Pan;
            }
        }

        public void OnKeyUp()
        {
            if (!Enabled)
            {
                return;
            }

            state = previousState;
            //window.addeventlistener 'keydown'
        }

        public void OnMouseDown(MouseButtons button, Vector2 page)
        {
            if (!Enabled)
            {
                return;
            }

            // stoppropagation

            if (state == State.None)
            {
                state = (State)button;
            }

            if (state == State.Rotate && RotateEnabled)
            {
                moveCurrent = GetMouseOnCircle(page);
                movePrevious = moveCurrent;
            }
            else if (state == State.Zoom && ZoomEnabled)
            {
                zoomStart = GetMouseOnScreen(page);
                zoomEnd = zoomStart;
            }
            else if (state == State.Pan && PanEnabled)
            {
                panStart = GetMouseOnScreen(page);
                panEnd = panStart;
            }

            //document.addEventListener('mousemove', mousemove, false);
            //document.addEventListener('mouseup', mouseup, false);

            //_this.dispatchEvent(startEvent);
        }

        public void OnMouseMove(Vector2 page)
        {
            if (!Enabled)
            {
                return;
            }

            // stoppropagation

            if (state == State.Rotate && RotateEnabled)
            {
                movePrevious = moveCurrent;
                moveCurrent = GetMouseOnCircle(page);
            }
            else if (state == State.Zoom && ZoomEnabled)
            {
                zoomEnd = GetMouseOnScreen(page);
            }
            else if (state == State.Pan && PanEnabled)
            {
                panEnd = GetMouseOnScreen(page);
            }
        }

        public void OnMouseUp()
        {
            if (!Enabled)
            {
                return;
            }

            state = State.None;

            //document.removeEventListener('mousemove', mousemove);
            //document.removeEventListener('mouseup', mouseup);
            //_this.dispatchEvent(endEvent);

        }

        public void OnMouseWheel(float delta)
        {
            if (!Enabled || !ZoomEnabled)
            {
                return;
            }

            // stop prop

            // zoomEnd?
            zoomStart.Y -= delta;
        }
    }
}
