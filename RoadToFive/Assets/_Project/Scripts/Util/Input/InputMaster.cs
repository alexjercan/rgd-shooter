// GENERATED AUTOMATICALLY FROM 'Assets/_Project/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace _Project.Scripts.Util.Input
{
    public class @InputMaster : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputMaster()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""c2056d22-7a96-4d33-a412-ae4289baef4c"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9860f76b-e661-404e-b797-dd742773418b"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""d3f9faa3-69fa-4b44-8a03-5d0b05b4057f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1a990abc-ef8e-4d53-9bf1-faafda2b5f17"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""c0cddd99-a539-46d9-872c-fe70eb3238b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleCursor"",
                    ""type"": ""Button"",
                    ""id"": ""857be815-e18c-49d4-a590-c6f87cd840ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ItemScroll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""30ca3105-6554-4b56-bb06-a8abb951329c"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""7749a0b1-c9d5-4c86-a47e-8e581b57ea69"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5d17366d-1440-41e0-b3ae-fb61b3be1189"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""88ecf01f-d0fb-4be2-9b93-d60ad1a088e9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8b7002cd-fb38-4e2c-ae4d-1260c444c30c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""16f9988c-e4f3-4feb-9cb5-1a40bf592558"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2669f874-a2e1-4be5-8e67-92a41369b22e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""MouseLook"",
                    ""id"": ""04b3c4c2-ced2-4256-954a-acdea121c04e"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a4fda25c-f684-4e20-9f2a-7cead50e0a14"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""59d55ee6-8c19-4c4a-b98e-b21021ff36d9"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""562bbfbb-d5d5-4142-b118-5d99598bc7b3"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2212100d-6cd7-4f01-bb97-368e4f416df0"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ffcccf88-3307-40bd-ac1f-1295cab4d0ff"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ea2ecdc-8f2c-4ae1-a0ae-55ea51683b38"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""KeyBoard"",
                    ""id"": ""336e2f7a-f842-40b0-bc24-57074323e3f3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemScroll"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a096ce58-7618-4cf4-8183-6bc1cda05549"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c938b7c1-1085-4914-b62b-94de0cb55adb"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ItemScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
            m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
            m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
            m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
            m_Player_ToggleCursor = m_Player.FindAction("ToggleCursor", throwIfNotFound: true);
            m_Player_ItemScroll = m_Player.FindAction("ItemScroll", throwIfNotFound: true);
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

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Movement;
        private readonly InputAction m_Player_Jump;
        private readonly InputAction m_Player_Look;
        private readonly InputAction m_Player_Shoot;
        private readonly InputAction m_Player_ToggleCursor;
        private readonly InputAction m_Player_ItemScroll;
        public struct PlayerActions
        {
            private @InputMaster m_Wrapper;
            public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_Player_Movement;
            public InputAction @Jump => m_Wrapper.m_Player_Jump;
            public InputAction @Look => m_Wrapper.m_Player_Look;
            public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
            public InputAction @ToggleCursor => m_Wrapper.m_Player_ToggleCursor;
            public InputAction @ItemScroll => m_Wrapper.m_Player_ItemScroll;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                    @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                    @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                    @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                    @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                    @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                    @ToggleCursor.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleCursor;
                    @ToggleCursor.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleCursor;
                    @ToggleCursor.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleCursor;
                    @ItemScroll.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemScroll;
                    @ItemScroll.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemScroll;
                    @ItemScroll.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnItemScroll;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @Shoot.started += instance.OnShoot;
                    @Shoot.performed += instance.OnShoot;
                    @Shoot.canceled += instance.OnShoot;
                    @ToggleCursor.started += instance.OnToggleCursor;
                    @ToggleCursor.performed += instance.OnToggleCursor;
                    @ToggleCursor.canceled += instance.OnToggleCursor;
                    @ItemScroll.started += instance.OnItemScroll;
                    @ItemScroll.performed += instance.OnItemScroll;
                    @ItemScroll.canceled += instance.OnItemScroll;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
        public interface IPlayerActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnShoot(InputAction.CallbackContext context);
            void OnToggleCursor(InputAction.CallbackContext context);
            void OnItemScroll(InputAction.CallbackContext context);
        }
    }
}
