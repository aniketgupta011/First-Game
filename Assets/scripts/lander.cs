using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;
    public static lander Instance {get; private set;} 

    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler OnFuelPickup;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs:EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }
    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }
    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }
    private Rigidbody2D landerRigidbody2D;
    private float fuelamount;
    private float fuelAmountMax = 10f;
    public State state;

    private void Awake()
    {
        Instance = this;
        fuelamount = fuelAmountMax;
        state= State.WaitingToStart;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;

        /*Debug.Log(Vector2.Dot(new Vector2(0,1), new Vector2(0,1)));
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(.5f,.5f)));
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(1, 0)));
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(0, -1)));*/
    }
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch(state)
        {
            default:
                case State.WaitingToStart:
                if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.IsRightActionPressed() || GameInput.Instance.GetMovementInputVector2() != Vector2.zero)
                { 
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);
                }
                break;
                case State.Normal:
        if(fuelamount<=0f)
        {
            return;
        }

        if(GameInput.Instance.IsUpActionPressed() || GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.IsRightActionPressed() || GameInput.Instance.GetMovementInputVector2() != Vector2.zero)
        {
            ConsumeFuel();
        }
                float gamepadDeadzone = .2f;
        if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.GetMovementInputVector2().y > gamepadDeadzone)
        {
            float force = 700f;
            landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }
        if (GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.GetMovementInputVector2().x < -gamepadDeadzone)
        {
            float TurnSpeed = +100f;
            landerRigidbody2D.AddTorque(TurnSpeed * Time.deltaTime);;
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (GameInput.Instance.IsRightActionPressed() || GameInput.Instance.GetMovementInputVector2().x > gamepadDeadzone)
        {
            float TurnSpeed = -100f;
            landerRigidbody2D.AddTorque(TurnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
                break;
            case State.GameOver:
                break;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on the Terrain!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
        float softlandingvelcotiymagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        if(relativeVelocityMagnitude > softlandingvelcotiymagnitude)
        {
            Debug.Log("Landed too hard!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }
        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if(dotVector<minDotVector)
        {
            Debug.Log("Landed on a too steep angle!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            SetState(State.GameOver);
            return;
        }

        Debug.Log("Successful landing!");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectormultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectormultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softlandingvelcotiymagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log("LandingAngleScore: " + landingAngleScore);
        Debug.Log("LandingSpeedScore: " + landingSpeedScore);

        int score = Mathf.RoundToInt((landingSpeedScore + landingAngleScore) * landingPad.GetScoreMultiplier());
        Debug.Log("Score: " + score);
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score,
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelamount += addFuelAmount;
            if(fuelamount>fuelAmountMax)
            {
                fuelamount=fuelAmountMax;
            }
            OnFuelPickup?.Invoke(this, EventArgs.Empty);
            fuelPickup.Destroyself();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.Destroyself();
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state }) ;
    }
    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelamount -= fuelConsumptionAmount * Time.deltaTime;
    }

    public float GetFuel()
    {
        return fuelamount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelamount / fuelAmountMax; 
    }

    public float GetSpeedX()
    {
        return landerRigidbody2D.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return landerRigidbody2D.linearVelocityY;
    }
}
