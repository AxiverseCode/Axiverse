using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using Axiverse.Interface.Graphics;
using Axiverse.Interface.Windows;

namespace Axiverse.Interface
{
    public class TrackballController
    {
        ///**
        // * @author Eberhard Graether / http://egraether.com/
        // * @author Mark Lundin 	/ http://mark-lundin.com
        // * @author Simone Manini / http://daron1337.github.io
        // * @author Luca Antiga 	/ http://lantiga.github.io
        // */
        
        public enum State
        {
            None = -1,
            Rotate = 0,
            Zoom = 1,
            Pan = 2,
            TouchRotate = 3,
            TouchZoomPan = 4,
        }

        public ICamera Camera;
        public Control Control;

        public Vector3 Position
        {
            get => new Vector3(Camera.Position.X, Camera.Position.Y, Camera.Position.Z);
            set => Camera.Position = new SharpDX.Vector3(value.X, value.Y, value.Z);
        }
        public Vector3 Up
        {
            get => new Vector3(Camera.Up.X, Camera.Up.Y, Camera.Up.Z);
            set => Camera.Up = new SharpDX.Vector3(value.X, value.Y, value.Z);
        }
        private Vector3 Target
        {
            get => new Vector3(Camera.Target.X, Camera.Target.Y, Camera.Target.Z);
            set => Camera.Target = new SharpDX.Vector3(value.X, value.Y, value.Z);
        }

        public bool Enabled { get; set; } = true;
        public Rectangle Screen;

        public float RotateSpeed { get; set; } = 4;
        public float ZoomSpeed { get; set; } = 1.2f;
        public float PanSpeed { get; set; } = 0.3f;

        public bool CanRotate { get; set; } = true;
        public bool CanZoom { get; set; } = true;
        public bool CanPan { get; set; } = true;

        public bool StaticMoving { get; set; } = true;
        public float DynamicDampingFactor { get; set; } = 0.2f;

        public float MinimumDistance { get; set; } = 0;
        public float MaximumDistance { get; set; } = float.PositiveInfinity;

        //	this.keys = [ 65 /*A*/, 83 /*S*/, 68 /*D*/ ];
        

        private float eps = 0.000001f;

        private Vector3 lastPosition;

        private State state = State.None;
        private State lastState = State.None;

        private Vector3 eye;

        private Vector2 lastMove;
        private Vector2 currentMove;

        private Vector3 lastAxis;
        private float lastAngle;

        private Vector2 zoomStart;
        private Vector2 zoomEnd;

        private float touchZoomDistanceStart;
        private float touchZoomDistanceEnd;

        private Vector2 panStart;
        private Vector2 panEnd;

        private Vector3 initialTarget;
        private Vector3 initialPosition;
        private Vector3 initialUp;

        public EventHandler Changed;
        public EventHandler Start;
        public EventHandler End;

        public TrackballController(ICamera camera, Control control)
        {
            Camera = camera;
            Control = control;

            Control.MouseDown += OnMouseDown;
            Control.SizeChanged += (sender, e) => Resize(new Rectangle(0, 0, Control.Width, control.Height));
            Control.MouseWheel += (sender, e) => OnMouseWheel(e);
            Screen = new Rectangle(0, 0, Control.Width, Control.Height);

            initialPosition = new Vector3(Camera.Position.X, Camera.Position.Y, Camera.Position.Z);
            initialTarget = new Vector3(Camera.Target.X, Camera.Target.Y, Camera.Target.Z);
            initialUp = new Vector3(Camera.Up.X, Camera.Up.Y, Camera.Up.Z);

            
        }

        public void Resize(Rectangle bounds)
        {
            Screen = bounds;
        }

        protected Vector2 GetMouseOnScreen(float x, float y)
        {
            return new Vector2((x - Screen.X) / Screen.W, (y - Screen.Y) / Screen.H);
        }

        protected Vector2 GetMouseOnCircle(float x, float y)
        {
            return new Vector2(
                ((x - Screen.W / 2 - Screen.X) / (Screen.W / 2)),
                ((Screen.H + 2 * (Screen.Y - y))/ Screen.W)); // Screen.W intentional
        }

        protected void RotateCamera()
        {
            Vector3 axis;
            Quaternion quaternion;
            Vector3 eyeDirection;
            Vector3 upDirection;
            Vector3 sideDirection;
            Vector3 moveDirection;
            float angle;

            moveDirection = new Vector3(currentMove.X - lastMove.X, currentMove.Y - lastMove.Y, 0);
            angle = moveDirection.Length();

            if (angle != 0)
            {
                eye = Position - Target;
                eyeDirection = eye.Normal();
                upDirection = Up.Normal();
                sideDirection = (upDirection % eyeDirection).Normal();

                upDirection.SetLength(currentMove.Y - lastMove.Y);
                sideDirection.SetLength(currentMove.X - lastMove.X);

                moveDirection = upDirection + sideDirection;
                axis = (moveDirection % eye).Normal();
                angle *= RotateSpeed;
                quaternion = Quaternion.FromAxisAngle(axis, angle);

                eye = quaternion.Transform(eye);
                Up = quaternion.Transform(Up).Normal();

                lastAxis = axis;
                lastAngle = angle;
            }
            else if (!StaticMoving && lastAngle != 0)
            {
                lastAngle *= Functions.Sqrt(1 - DynamicDampingFactor);
                eye = Position - Target;
                quaternion = Quaternion.FromAxisAngle(lastAxis, lastAngle);

                eye = quaternion.Transform(eye);
                Up = quaternion.Transform(Up);
            }

            lastMove = currentMove;
        }

        protected void ZoomCamera()
        {
            float factor;

            if (state == State.TouchZoomPan)
            {
                factor = touchZoomDistanceStart / touchZoomDistanceEnd;
                eye = eye * factor;
            }
            else
            {
                factor = 1 + (zoomEnd.Y - zoomStart.Y) * ZoomSpeed;

                if (factor != 1 && factor > 0)
                {
                    eye = eye * factor;
                }

                if (StaticMoving)
                {
                    zoomStart = zoomEnd;
                }
                else
                {
                    zoomStart.Y += zoomEnd.Y - zoomStart.Y * DynamicDampingFactor;
                }
            }
        }

        protected void PanCamera()
        {
            Vector2 mouseChange;
            Vector3 up;
            Vector3 pan;

            mouseChange = panEnd - panStart;

            if (mouseChange.LengthSquared() != 0)
            {
                mouseChange *= eye.Length() * PanSpeed;
                up = Up.Clone().OfLength(mouseChange.Y);
                pan = (eye % Up).OfLength(mouseChange.X);
                pan = pan + up;

                Position += pan;
                Target += pan;

                if (StaticMoving)
                {
                    panStart = panEnd;
                }
                else
                {
                    panStart = panStart + (panEnd - panStart) * DynamicDampingFactor;
                }
            }
        }

        protected void CheckDistances()
        {
            if (CanZoom || CanPan)
            {
                if (eye.LengthSquared() > MaximumDistance * MaximumDistance)
                {
                    Position = Target + eye.OfLength(MaximumDistance);
                    zoomStart = zoomEnd;
                }

                if (eye.LengthSquared() < MinimumDistance * MinimumDistance)
                {
                    Position = Target + eye.OfLength(MinimumDistance);
                    zoomStart = zoomEnd;
                }
            }
        }

        public void Update()
        {
            eye = Position - Target;

            if (CanRotate)
            {
                RotateCamera();
            }

            if (CanZoom)
            {
                ZoomCamera();
            }

            if (CanPan)
            {
                PanCamera();
            }

            Position = Target + eye;
            CheckDistances();

            //LookAt(target);
            //Control.Children[0].Text = $"{Camera.Position} -> {Camera.Target}";

            if (lastPosition.DistanceToSquared(Position) > eps)
            {
                Changed?.Invoke(this, null);
                lastPosition = Position;
            }
        }

        public void Reset()
        {
            state = State.None;
            lastState = State.None;

            Target = initialTarget;
            Position = initialPosition;
            Up = initialUp;

            eye = Position - Target;
            //LookAt(target);

            Changed(this, null);
            lastPosition = Position;
        }

        public void OnKeyDown()
        {
            if (!Enabled)
            {
                return;
            }

            //RemoveListener(OnKeyDown());

            //lastState = state;

            //if (state != State.None)
            //{
            //    return;
            //}

            //if (e.Key = RotateKey && CanRotate)
            //{
            //    state = State.Rotate;
            //}
            //else if (e.Key = ZoomKey && CanZoom)
            //{
            //    state = State.Zoom;
            //}
            //else if (e.Key = PanKey && CanPan)
            //{
            //    state = State.Pan;
            //}
        }

        public void OnKeyUp()
        {
            if(!Enabled)
            {
                return;
            }

            state = lastState;
            //AddListener(OnKeyDown)
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            float x = e.X;
            float y = e.Y;

            if (!Enabled)
            {
                return;
            }

            if (state == State.None)
            {
                switch(e.Button)
                {
                    case MouseButtons.Primary:
                        state = State.Rotate;
                        break;
                    case MouseButtons.Secondary:
                        state = State.Pan;
                        break;
                    case MouseButtons.Tertiary:
                        state = State.Zoom;
                        break;
                }
            }

            if (state == State.Rotate && CanRotate)
            {
                currentMove = GetMouseOnCircle(x, y);
                lastMove = currentMove;
            }
            else if (state == State.Zoom && CanZoom)
            {
                zoomStart = GetMouseOnScreen(x, y);
                zoomEnd = zoomStart;
            }
            else if (state == State.Pan && CanPan)
            {
                panStart = GetMouseOnScreen(x, y);
                panEnd = panStart;
            }

            Control.MouseUp += OnMouseUp;
            Control.MouseMove += OnMouseMove;
            //OnStart();
        }

        public void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            float x = e.X;
            float y = e.Y;
            //Control.Children[0].Text = $"{x}, {y}";

            if (!Enabled)
            {
                return;
            }

            if (state == State.Rotate && CanRotate)
            {
                lastMove = currentMove;
                currentMove = GetMouseOnCircle(x, y);
            }
            else if (state == State.Zoom && CanZoom)
            {
                zoomEnd = GetMouseOnScreen(x, y);
            }
            else if (state == State.Pan && CanPan)
            {
                panEnd = GetMouseOnScreen(x, y);
            }
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            state = State.None;
            Control.MouseUp -= OnMouseUp;
            Control.MouseMove -= OnMouseMove;
        }

        public void OnMouseWheel(MouseMoveEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }
            

            //		switch ( event.deltaMode ) {

            //                        case 2:
            //                                // Zoom in pages
            //                                _zoomStart.y -= event.deltaY * 0.025;
            //                                break;

            //			case 1:
            //                                // Zoom in lines
            //				_zoomStart.y -= event.deltaY * 0.01;
            //				break;

            //			default:
            //				// undefined, 0, assume pixels
            //				_zoomStart.y -= event.deltaY * 0.00025;
            //				break;

            //		}

            zoomStart.Y -= e.DeltaZ * 0.00025f;
        }



        //	function touchstart( event ) {

        //		if ( _this.enabled === false ) return;

        //		switch ( event.touches.length ) {

        //			case 1:
        //				_state = STATE.TOUCH_ROTATE;
        //				_moveCurr.copy( getMouseOnCircle( event.touches[ 0 ].pageX, event.touches[ 0 ].pageY ) );
        //				_movePrev.copy( _moveCurr );
        //				break;

        //			default: // 2 or more
        //				_state = STATE.TOUCH_ZOOM_PAN;
        //				var dx = event.touches[ 0 ].pageX - event.touches[ 1 ].pageX;
        //				var dy = event.touches[ 0 ].pageY - event.touches[ 1 ].pageY;
        //				_touchZoomDistanceEnd = _touchZoomDistanceStart = Math.sqrt( dx * dx + dy * dy );

        //				var x = ( event.touches[ 0 ].pageX + event.touches[ 1 ].pageX ) / 2;
        //				var y = ( event.touches[ 0 ].pageY + event.touches[ 1 ].pageY ) / 2;
        //				_panStart.copy( getMouseOnScreen( x, y ) );
        //				_panEnd.copy( _panStart );
        //				break;

        //		}

        //		_this.dispatchEvent( startEvent );

        //	}

        //	function touchmove( event ) {

        //		if ( _this.enabled === false ) return;

        //		event.preventDefault();
        //		event.stopPropagation();

        //		switch ( event.touches.length ) {

        //			case 1:
        //				_movePrev.copy( _moveCurr );
        //				_moveCurr.copy( getMouseOnCircle( event.touches[ 0 ].pageX, event.touches[ 0 ].pageY ) );
        //				break;

        //			default: // 2 or more
        //				var dx = event.touches[ 0 ].pageX - event.touches[ 1 ].pageX;
        //				var dy = event.touches[ 0 ].pageY - event.touches[ 1 ].pageY;
        //				_touchZoomDistanceEnd = Math.sqrt( dx * dx + dy * dy );

        //				var x = ( event.touches[ 0 ].pageX + event.touches[ 1 ].pageX ) / 2;
        //				var y = ( event.touches[ 0 ].pageY + event.touches[ 1 ].pageY ) / 2;
        //				_panEnd.copy( getMouseOnScreen( x, y ) );
        //				break;

        //		}

        //	}

        //	function touchend( event ) {

        //		if ( _this.enabled === false ) return;

        //		switch ( event.touches.length ) {

        //			case 0:
        //				_state = STATE.NONE;
        //				break;

        //			case 1:
        //				_state = STATE.TOUCH_ROTATE;
        //				_moveCurr.copy( getMouseOnCircle( event.touches[ 0 ].pageX, event.touches[ 0 ].pageY ) );
        //				_movePrev.copy( _moveCurr );
        //				break;

        //		}

        //		_this.dispatchEvent( endEvent );

        //	}

    }

}
