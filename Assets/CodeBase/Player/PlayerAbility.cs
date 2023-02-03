using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace CodeBase.Player
{
    public class PlayerAbility : NetworkBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private NetworkAnimator networkAnimator;
        [SerializeField] private Material hitMaterial;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private List<SkinnedMeshRenderer> allBody;
        [SerializeField] private CharacterController character;
        [SerializeField] private Scoreboard scoreboard;
        [SerializeField] private int currentScorePlayer;

        [SyncVar] [SerializeField, Range(10, 250)]
        private float powerDash;

        [SyncVar] [SerializeField] private float cooldownDash;
        [SyncVar] [SerializeField] private float timeBeHitted;
        [SyncVar] private bool _wasHit;
        [SyncVar] private bool _isActiveAbility;
        [SyncVar] private PlayerAbility _currentHittedOpponent;

        private float _elapsedCooldownDash;
        private Vector3 _impact = Vector3.zero;
        private const float Mass = 1f;
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        private void Start()
        {
            _elapsedCooldownDash = cooldownDash;
            GetScoreboard();
        }


        private void Update()
        {
            if (!isLocalPlayer) return;

            Move();
            if (Input.GetMouseButtonDown(0) && !_isActiveAbility)
                AddImpact(playerMovement.currentDirection, powerDash);

            if (_isActiveAbility)
            {
                DecreaseCooldownDash();
                if (PassedCooldownAbility())
                {
                    ResetAbility();
                    ResetCooldownDash();
                }
            }

            if (_wasHit)
            {
                DecreaseCooldownHit();
                if (PassedCooldownHit())
                {
                    ResetAfterHitCor(_currentHittedOpponent);
                }
            }

            ConsumeEnergy();
        }

        private void Move()
        {
            if (_impact.magnitude > 0.2f)
                character.Move(_impact * Time.deltaTime);
        }

        private async void OnTriggerEnter(Collider other)
        {
            if (!isLocalPlayer) return;
            if (other.TryGetComponent(out PlayerAbility opponent) && opponent != this)
            {
                if (_isActiveAbility && !opponent._wasHit)
                {
                    await RegisterHit(opponent);
                    Attack(opponent);
                    UpdateScore();
                    ClientUpdateKills();
                    scoreboard.UpdateScoreText(currentScorePlayer + 1);
                }
            }
        }

        [Command]
        private void Attack(PlayerAbility opponent) => ChangeColor(opponent);

        [ClientRpc]
        private void ChangeColor(PlayerAbility opponent)
        {
            for (int i = 0; i < opponent.allBody.Count; i++)
            {
                opponent.allBody[i].material = hitMaterial;
            }

            opponent._wasHit = true;
            opponent._currentHittedOpponent = opponent;
        }

        private async Task RegisterHit(PlayerAbility opponent)
        {
            opponent._wasHit = true;
            await Task.CompletedTask;
        }

        [ClientRpc]
        private void ResetColor(PlayerAbility opponent)
        {
            for (int i = 0; i < opponent.allBody.Count; i++)
            {
                opponent.allBody[i].material = defaultMaterial;
            }

            opponent._wasHit = false;
            _currentHittedOpponent = null;
            timeBeHitted = 3f;
        }

        private void AddImpact(Vector3 dir, float force)
        {
            _isActiveAbility = true;
            dir.Normalize();
            if (dir.y < 0)
                dir.y = -dir.y;
            _impact += dir.normalized * force / Mass;
            networkAnimator.SetTrigger(IsAttack);
        }

        [ClientRpc]
        private void ShowWinner()
        {
            if (currentScorePlayer == 3)
            {
                scoreboard.ActivateWinUi();
                Invoke(nameof(ResetAHost), 5f);
            }
        }

        [Command]
        private void ResetAfterHitCor(PlayerAbility opponent) => ResetColor(opponent);

        [Command]
        private void UpdateScore() => IncreaseKills();

        [ClientRpc]
        private void IncreaseKills() => currentScorePlayer++;

        [Command]
        private void ClientUpdateKills() => ShowWinner();

        private void ConsumeEnergy() => _impact = Vector3.Lerp(_impact, Vector3.zero, 5 * Time.deltaTime);
        private void ResetAbility() => _isActiveAbility = false;
        private void DecreaseCooldownHit() => timeBeHitted -= Time.deltaTime;
        private void DecreaseCooldownDash() => _elapsedCooldownDash -= Time.deltaTime;
        private void ResetCooldownDash() => _elapsedCooldownDash = cooldownDash;
        private bool PassedCooldownHit() => timeBeHitted <= 0f;
        private bool PassedCooldownAbility() => _elapsedCooldownDash <= 0f;
        private void GetScoreboard() => scoreboard = FindObjectOfType<Scoreboard>();
        private void ResetAHost() => NetworkManager.singleton.StopHost();
    }
}