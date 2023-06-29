using UnityEngine;

public class PlayerStatusWatcher : MonoBehaviour{
    private Player _player;
    private Rigidbody2D _playerRigidBody;

    #region Status bools
    public bool isDashDone {
        get;
        set;
    }
    public bool isInAir{
        get{
            int groundLayerMask = LayerMask.GetMask("Ground");
            Vector2 direction = -_playerRigidBody.transform.up;
            Vector3 offset = new Vector3(0, 0.85f, 0);
            Vector3 origin = _playerRigidBody.transform.position - offset;
            Vector2 size = new Vector2(1, 0.1f);
            float angle = 0f;
            float distance = 1f;
            RaycastHit2D hit = Physics2D.BoxCast(origin, size, angle, direction, distance, groundLayerMask);
            bool isInAir = hit.collider == null;
            return isInAir;
        }
    }
    public bool isStandingOnGround{
        get{
            return _playerRigidBody.velocity == Vector2.zero && !isInAir;
        }
    } 
    public bool isRunning {
        get{
            return isMoving || _playerRigidBody.velocity.x != 0 && _playerRigidBody.velocity.y == 0;
        }
    }
    public bool isJumping {
        get{
            return jumpKeyPressed && !isInAir && !hasJumped;
        }
    }
    public bool dashKeyPressed {
        get{
            return Input.GetKeyDown(KeyCode.LeftShift);
        }
    }
    public bool jumpKeyPressed {
        get{
            return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);
        }
    }
    public bool isMoving {
        get{
            return Input.GetAxisRaw("Horizontal") != 0;
        }
    }
    public bool hasJumped {
        get; 
        set;
    }
    public bool isNotMoving{
        get{
            return _playerRigidBody.velocity == Vector2.zero;
        }
    }
    #endregion

    private void Awake() {
        _player = GetComponent<Player>();
        _playerRigidBody = GetComponent<Rigidbody2D>();
    }

}
