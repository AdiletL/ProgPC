using Common.StateMachine;
using UnityEngine;
using Zenject;

namespace Unit.Character.Player
{
    public class PlayerBuildState : State, IBuildMode
    {
        [Inject] private DiContainer diContainer;
        [Inject] private SO_GameColor so_GameColor;
        [Inject] private SO_GameConfig so_GameConfig;
        
        public override StateCategory Category { get; } = StateCategory.Build;
        
        private SO_PlayerBuild so_PlayerBuild;
        private Camera playerCamera;
        
        
        private GameObject currentBuildableObject;
        private IBuildable currentBuildable;
        private LayerMask currentPlacementSurfaceLayer;
        
        private Color validatePlacementColor;
        private Color invalidatePlacementColor;
        
        private Vector3 currentExtents;
        
        private float maxPlacementDistance;
        private float maxPreviewDistance;
        private float angleRotation;
        private bool isValidatePosition;
        private bool isCollisionBuildableObject;
        
        public void SetConfig(SO_PlayerBuild so_PlayerBuild) => this.so_PlayerBuild = so_PlayerBuild;
        public void SetPlayerCamera(Camera camera) => this.playerCamera = camera;
        
        private float CalculateOffset(Vector3 surfaceNormal)
        {
            return Vector3.Project(currentExtents, surfaceNormal).magnitude;
        }

        public override void Initialize()
        {
            base.Initialize();
            maxPlacementDistance = so_PlayerBuild.MaxPlacementDistance;
            maxPreviewDistance = so_PlayerBuild.MaxPreviewDistance;
            validatePlacementColor = so_GameColor.ValidatePlacement;
            invalidatePlacementColor = so_GameColor.InvalidatePlacement;
            angleRotation = so_GameConfig.AngleRotate;
        }

        public override void Enter()
        {
            base.Enter();
            IsCanExit = false;
        }

        public override void Update()
        {
            if (currentBuildableObject)
            {
                UpdateObjectPosition();
                ProcessRotation();
                if (Input.GetMouseButtonDown(0) 
                    && isValidatePosition)
                {
                    PlaceObject();
                }
            }
        }
        

        public void SetBuildableObject(GameObject buildableGameObject)
        {
            if (currentBuildableObject)
                ClearCurrentBuildableObject();

            currentBuildableObject = buildableGameObject;
            currentBuildableObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * 3f;
            currentBuildableObject.transform.rotation = Quaternion.identity;
            
            currentBuildable = currentBuildableObject.GetComponent<Gameplay.BuildableObject>();
            currentBuildable.ExecutePreview();
            currentPlacementSurfaceLayer = currentBuildable.SurfacePlacementLayer;
            currentBuildable.OnCollisionBuildableObject += OnCollisionBuildableObject;
            
            Collider col = currentBuildableObject.GetComponent<Collider>();
            if (col != null)
            {
                currentExtents = col.bounds.extents;
            }
            else
            {
                Renderer rend = currentBuildableObject.GetComponent<Renderer>();
                currentExtents = rend != null ? rend.bounds.extents : Vector3.one * 0.1f;
            }
        }
        
        private void UpdateObjectPosition()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxPlacementDistance, currentPlacementSurfaceLayer, QueryTriggerInteraction.Ignore))
            {
                if (currentBuildable.TryGetPlacementPosition(hit, currentBuildableObject, out Vector3 pos))
                {
                    currentBuildable.ChangePosition(pos);
                    currentBuildable.ChangeColor(validatePlacementColor);
                    isValidatePosition = true;
                    return;
                }

                Vector3 normal = hit.normal;
                float offset = CalculateOffset(normal);
                
                Vector3 position = hit.point + normal * offset;
                currentBuildable.ChangePosition(position);
                
                if (hit.collider.TryGetComponent(out IBuildable buildable)
                    || isCollisionBuildableObject)
                {
                    currentBuildable.ChangeColor(invalidatePlacementColor);
                    isValidatePosition = false;
                }
                else
                {
                    currentBuildable.ChangeColor(validatePlacementColor);
                    isValidatePosition = true;
                }
            }
            else
            {
                Vector3 position = playerCamera.transform.position + playerCamera.transform.forward * maxPreviewDistance;
                currentBuildable.ChangePosition(position);
                currentBuildable.ResetColor();
                isValidatePosition = false;
            }
        }
        
        private void ProcessRotation()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
            if (scrollInput != 0)
            {
                float rotationAmount = scrollInput > 0 ? angleRotation : -angleRotation;
                currentBuildable.ChangeRotate(rotationAmount);
            }
        }

        private void PlaceObject()
        {
            Collider[] colliders = Physics.OverlapSphere(currentBuildableObject.transform.position, .1f, currentPlacementSurfaceLayer);
            if(colliders.Length > 0 && colliders[0].gameObject != currentBuildableObject) return;
            currentBuildable.Build();
            currentBuildable = null;
            currentBuildableObject = null;
            isCollisionBuildableObject = false;
            IsCanExit = true;
            StateMachine.ExitCategory(Category, null);
        }
        
        private void ClearCurrentBuildableObject()
        {
            if (!currentBuildableObject) return;
        
            currentBuildable.OnCollisionBuildableObject -= OnCollisionBuildableObject;
            Object.Destroy(currentBuildableObject);
            currentBuildable = null;
            currentBuildableObject = null;
            isCollisionBuildableObject = false;
        }

        private void OnCollisionBuildableObject(GameObject obj)
        {
            if(currentBuildable == null) return;
            isCollisionBuildableObject = obj;
        }
    }

    public class PlayerBuildStateBuilder : StateBuilder
    {
        public PlayerBuildStateBuilder() : base(new PlayerBuildState())
        {
        }

        public PlayerBuildStateBuilder SetConfig(SO_PlayerBuild so_PlayerBuild)
        {
            if(state is PlayerBuildState playerBuildState)
                playerBuildState.SetConfig(so_PlayerBuild);
            return this;
        }
        public PlayerBuildStateBuilder SetPlayerCamera(Camera camera)
        {
            if(state is PlayerBuildState playerBuildState)
                playerBuildState.SetPlayerCamera(camera);
            return this;
        }
    }
}