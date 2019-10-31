using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SharpDX.XInput;

namespace RC_Car_Remote_Controller
{
    class ps4controller
    {
        private const int RefreshRate = 60;

        public float m_LeftThumbX;
        public float m_LeftThumbY;

        public float m_RightThumbX;
        public float m_RightThumbY;

        public float m_RightTrigger;
        public float m_LeftTrigger;

        public bool m_A;
        public bool m_B;
        public bool m_X;
        public bool m_Y;

        public bool m_DPadUp;
        public bool m_DPadDown;
        public bool m_DPadRight;
        public bool m_DPadLeft;

        public bool m_LeftShoulder;
        public bool m_RightShoulder;

        public bool m_Back;
        public bool m_Start;
        public bool m_RightThumb;
        public bool m_LeftThumb;

        private Timer _timer;
        private Controller _controller;
        public Vibration _v;

        public ps4controller()
        {
            _controller = new Controller(UserIndex.One);
            _timer = new Timer(obj => Update());
            _v = new Vibration();
        }

        public void Start()
        {
            _timer.Change(0, 1000 / RefreshRate);
        }

        private void Update()
        {
            _controller.GetState(out var state);
            Inputy(state);
        }

        public void Inputy(State state)
        {
            m_LeftThumbX = state.Gamepad.LeftThumbX;
            m_LeftThumbY = state.Gamepad.LeftThumbY;
            m_RightThumbX = state.Gamepad.RightThumbX;
            m_RightThumbY = state.Gamepad.RightThumbY;
            m_RightTrigger = state.Gamepad.RightTrigger;
            m_LeftTrigger = state.Gamepad.LeftTrigger;
            m_LeftShoulder = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder);
            m_RightShoulder = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder);
            m_B = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
            m_A = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
            m_X = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.X);
            m_Y = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y);
            m_DPadUp = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp);
            m_DPadDown = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown);
            m_DPadRight = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight);
            m_DPadLeft = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft);
            m_Back = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Back);
            m_Start = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Start);
            m_RightThumb = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb);
            m_LeftThumb = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb);
            /*
            _v.LeftMotorSpeed = 65000;
            _v.RightMotorSpeed = 65000;
            _controller.SetVibration(_v);
            */
        }
    }
}
