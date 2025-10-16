using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private GameObject _interactionIcon;
    [SerializeField] private Image _holdProgressBar;
    private List<IInteractable> _interactablesInRange = new List<IInteractable>();
    private IInteractable _currentTarget;
    private CircleCollider2D _pickUpRange;
    private float _holdTimer = 0f;
    private bool _isHolding = false;
    private bool _sfxPlayed = false;

    public float holdDuration = 1f;
    public float pickUpRadius;

    void Start()
    {
        _interactionIcon.SetActive(false);
        _holdProgressBar.gameObject.SetActive(false);
        _pickUpRange = GetComponent<CircleCollider2D>();
        pickUpRadius = _pickUpRange.radius;
    }

    void Update()
    {
        if (_isHolding && _currentTarget != null)
        {
            if (!_sfxPlayed)
            {
                AudioManager.instance.PlayPickUpTrash();
                _sfxPlayed = true;
            }

            _holdTimer += Time.deltaTime;
            _holdProgressBar.fillAmount = _holdTimer / holdDuration;

            if (_holdTimer >= holdDuration)
            {
                _currentTarget.Interact();
                _isHolding = false;
                _holdTimer = 0f;
                _holdProgressBar.gameObject.SetActive(false);
                _holdProgressBar.fillAmount = 0f;
                _sfxPlayed = false;
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (_currentTarget == null) return;

        // If interaction requires hold activate the holding mechanics
        if (_currentTarget.RequiresHold)
        {
            if (context.started)
            {
                if (_currentTarget != null)
                {
                    _isHolding = true;
                    _holdTimer = 0f;
                    _holdProgressBar.gameObject.SetActive(true);
                }
            }
            else if (context.canceled)
            {
                _isHolding = false;
                _holdTimer = 0f;
                _holdProgressBar.gameObject.SetActive(false);
                _holdProgressBar.fillAmount = 0f;
            }
        }
        // Else, just straight to interact
        else
        {
            if(context.performed)
            {
                _currentTarget.Interact();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            _interactablesInRange.Add(interactable);
            _currentTarget = GetClosestInteractable();
            _interactionIcon.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            _interactablesInRange.Remove(interactable);
            _currentTarget = GetClosestInteractable();

            if (_currentTarget == null)
                _interactionIcon.SetActive(false);
        }
    }

    private IInteractable GetClosestInteractable()
    {
        if (_interactablesInRange.Count == 0) return null;

        float minDist = float.MaxValue;
        IInteractable closest = null;

        foreach (var i in _interactablesInRange)
        {
            var go = (i as MonoBehaviour).gameObject;
            float dist = Vector2.Distance(transform.position, go.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = i;
            }
        }
        return closest;
    }

    public void UpdateCircleRange()
    {
        _pickUpRange.radius = pickUpRadius;
    }
}