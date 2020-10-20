// GENERATED AUTOMATICALLY FROM 'Assets/HellsChicken/Scripts/Game/Input/NewInputSystem.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @NewInputSystem : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @NewInputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""NewInputSystem"",
    ""maps"": [
        {
            ""name"": ""Walking"",
            ""id"": ""6cdf7552-26d3-46fb-800c-eeca56e0f201"",
            ""actions"": [
                {
                    ""name"": ""JumpAndGlide"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6dc977fb-c95a-4f4d-849a-1eaacd0a6ceb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9f9104c2-fb23-4213-a70c-3ffc7029756c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EggAiming"",
                    ""type"": ""PassThrough"",
                    ""id"": ""acf16bee-7b48-47b0-9a56-eeed24fd15ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShootFlame"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6e281560-905c-465a-aa9c-02af298e00c3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4f105f34-6d11-46b0-990b-526703ce3b4f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""JumpAndGlide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13d9f236-1c21-4401-a61a-e1c2ec7a3c89"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""JumpAndGlide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""AD"",
                    ""id"": ""b0c9db5f-9f81-44e4-89f5-795e5d1554b5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""506b37cd-7eda-4ec7-84e6-2fd7571856b5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""3192b945-c319-4b16-bb76-77f60810a50f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a3e9e943-95e6-45f9-b5df-5023664811fd"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""39c111a8-dc38-48bd-a064-acaf8904fa9a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""EggAiming"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a141774-da48-47e4-aa5f-6d0d5204eed2"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""EggAiming"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02cb7e5c-461f-4e30-b320-1f44f8f6ea68"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ShootFlame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2728a2dc-8cae-4333-b9f6-9f3408f35deb"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""XboxController"",
                    ""action"": ""ShootFlame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XboxController"",
            ""bindingGroup"": ""XboxController"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Walking
        m_Walking = asset.FindActionMap("Walking", throwIfNotFound: true);
        m_Walking_JumpAndGlide = m_Walking.FindAction("JumpAndGlide", throwIfNotFound: true);
        m_Walking_Move = m_Walking.FindAction("Move", throwIfNotFound: true);
        m_Walking_EggAiming = m_Walking.FindAction("EggAiming", throwIfNotFound: true);
        m_Walking_ShootFlame = m_Walking.FindAction("ShootFlame", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Walking
    private readonly InputActionMap m_Walking;
    private IWalkingActions m_WalkingActionsCallbackInterface;
    private readonly InputAction m_Walking_JumpAndGlide;
    private readonly InputAction m_Walking_Move;
    private readonly InputAction m_Walking_EggAiming;
    private readonly InputAction m_Walking_ShootFlame;
    public struct WalkingActions
    {
        private @NewInputSystem m_Wrapper;
        public WalkingActions(@NewInputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @JumpAndGlide => m_Wrapper.m_Walking_JumpAndGlide;
        public InputAction @Move => m_Wrapper.m_Walking_Move;
        public InputAction @EggAiming => m_Wrapper.m_Walking_EggAiming;
        public InputAction @ShootFlame => m_Wrapper.m_Walking_ShootFlame;
        public InputActionMap Get() { return m_Wrapper.m_Walking; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WalkingActions set) { return set.Get(); }
        public void SetCallbacks(IWalkingActions instance)
        {
            if (m_Wrapper.m_WalkingActionsCallbackInterface != null)
            {
                @JumpAndGlide.started -= m_Wrapper.m_WalkingActionsCallbackInterface.OnJumpAndGlide;
                @JumpAndGlide.performed -= m_Wrapper.m_WalkingActionsCallbackInterface.OnJumpAndGlide;
                @JumpAndGlide.canceled -= m_Wrapper.m_WalkingActionsCallbackInterface.OnJumpAndGlide;
                @Move.started -= m_Wrapper.m_WalkingActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_WalkingActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_WalkingActionsCallbackInterface.OnMove;
                @EggAiming.started -= m_Wrapper.m_WalkingActionsCallbackInterface.OnEggAiming;
                @EggAiming.performed -= m_Wrapper.m_WalkingActionsCallbackInterface.OnEggAiming;
                @EggAiming.canceled -= m_Wrapper.m_WalkingActionsCallbackInterface.OnEggAiming;
                @ShootFlame.started -= m_Wrapper.m_WalkingActionsCallbackInterface.OnShootFlame;
                @ShootFlame.performed -= m_Wrapper.m_WalkingActionsCallbackInterface.OnShootFlame;
                @ShootFlame.canceled -= m_Wrapper.m_WalkingActionsCallbackInterface.OnShootFlame;
            }
            m_Wrapper.m_WalkingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @JumpAndGlide.started += instance.OnJumpAndGlide;
                @JumpAndGlide.performed += instance.OnJumpAndGlide;
                @JumpAndGlide.canceled += instance.OnJumpAndGlide;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @EggAiming.started += instance.OnEggAiming;
                @EggAiming.performed += instance.OnEggAiming;
                @EggAiming.canceled += instance.OnEggAiming;
                @ShootFlame.started += instance.OnShootFlame;
                @ShootFlame.performed += instance.OnShootFlame;
                @ShootFlame.canceled += instance.OnShootFlame;
            }
        }
    }
    public WalkingActions @Walking => new WalkingActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_XboxControllerSchemeIndex = -1;
    public InputControlScheme XboxControllerScheme
    {
        get
        {
            if (m_XboxControllerSchemeIndex == -1) m_XboxControllerSchemeIndex = asset.FindControlSchemeIndex("XboxController");
            return asset.controlSchemes[m_XboxControllerSchemeIndex];
        }
    }
    public interface IWalkingActions
    {
        void OnJumpAndGlide(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnEggAiming(InputAction.CallbackContext context);
        void OnShootFlame(InputAction.CallbackContext context);
    }
}
