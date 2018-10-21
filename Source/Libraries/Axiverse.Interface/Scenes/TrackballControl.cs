using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axiverse.Interface.Scenes
{
    class TrackballControl
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

        public bool Enabled { get; set; }
        public Rectangle Screen { get; set; }
        public float RotateSpeed { get; set; }
        public float ZoomSpeed { get; set; }
        public float PanSpeed { get; set; }
        public bool RotateEnabled { get; set; }
        public bool ZoomEnabled { get; set; }
        public bool PanEnabled { get; set; }

        public bool StaticMoving { get; set; }
        public float DynamicDampeningFactor { get; set; }

        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }

        private Vector3 target;
        private float epsilon = 0.000001f;
        private Vector3 lastPosition;
        private State state =State.None;
        private State previousState = State.None;
        private Vector3 eye;
        private Vector2 movePrev;
        private Vector2 moveCurr;
        private Vector3 lastAxis;
        private float lastAngle;
        private Vector2 zoomStart;
        private Vector2 zoomEnd;
        private float touchZoomDistanceStart = 0;
        private float touchZoomDistanceEnd = 0;
        private Vector2 panStart;
        private Vector2 panEnd;

        private Vector3 objectPosition;
        private Vector3 objectUpDirection;
        private Vector3 objectSidewaysDirection;

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
                Screen.Height + 2 * (Screen.Top + page.Y) / Screen.Width);

        void RotateCamera()
        {
            Vector3 axis;
            Quaternion quaternion;
            Vector3 eyeDirection;
            Vector3 objectUpDirection;
            Vector3 objectSidewaysDirection;
            Vector3 moveDirection;
            float angle;

            moveDirection = new Vector3(moveCurr.X - movePrev.X, moveCurr.Y - movePrev.Y, 0);
            angle = moveDirection.Length();

            if (angle != 0)
            {
                eye = objectPosition - target;

                eyeDirection = eye.Normal();
                objectUpDirection = this.objectUpDirection.Normal();
                objectSidewaysDirection = (objectUpDirection % eyeDirection).Normal();

                moveDirection = objectUpDirection + objectSidewaysDirection;
                axis = (moveDirection % eyeDirection).Normal();

                angle *= RotateSpeed;
                quaternion = Quaternion.FromAxisAngle(axis, angle);

                eye = quaternion.Transform(eye);
                this.objectUpDirection = quaternion.Transform(this.objectUpDirection);

                lastAxis = axis;
                lastAngle = angle;
            }
            else if (!StaticMoving && lastAngle != 0)
            {
                lastAngle *= Functions.Sqrt(1 - DynamicDampeningFactor);
                eye = this.objectPosition - target;
                quaternion = Quaternion.FromAxisAngle(lastAxis, lastAngle);
                eye = quaternion.Transform(eye);
                this.objectUpDirection = quaternion.Transform(this.objectUpDirection);
            }
            movePrev = moveCurr;
        }

        void ZoomCamera ()
        {
            float factor;

            if (state == State.TouchZoomPan)
            {
                factor = touchZoomDistanceStart / touchZoomDistanceEnd;
                touchZoomDistanceStart = touchZoomDistanceEnd;
                eye *= factor;
            }
            else
            {
                factor = 1 + (zoomEnd.Y - zoomStart.Y) * ZoomSpeed;

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
            Vector2 mouseChange;
            Vector3 objectUp;
            Vector3 pan;

            mouseChange = panEnd - panStart;

            if (mouseChange != Vector2.Zero)
            {
                mouseChange *= eye.Length() * PanSpeed;
                pan = (eye % objectUpDirection).OfLength(mouseChange.X);
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

        void Update()
        {
            eye = objectPosition - target;

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

        void Reset()
        {
            state = State.None;
            previousState = State.None;

        }

        void OnKeyDown(char key)
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

        void OnKeyUp()
        {
            if (!Enabled)
            {
                return;
            }

            state = previousState;
            //window.addeventlistener 'keydown'
        }

        void OnMouseDown(int button, Vector2 page)
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
                moveCurr = GetMouseOnCircle(page);
                movePrev = moveCurr;
            }
            else if (state== State.Zoom && ZoomEnabled)
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

        void OnMouseMove(Vector2 page)
        {
            if (!Enabled)
            {
                return;
            }

            // stoppropagation
            
            if (state == State.Rotate && RotateEnabled)
            {
                movePrev = moveCurr;
                moveCurr = GetMouseOnCircle(page);
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

        void MouseUp()
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

        void OnMouseWheel(float delta)
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
