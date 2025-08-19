//using UnityEngine;

//public class HeroStateContext : StateContext<PlayerController, PlayerStateContext.PlayerState>
//{
//    public enum PlayerState
//    {
//        IDLE = 0,
//        MOVE = 1,
//        JUMP = 2,
//        WALL = 3,
//        BOARD = 4,
//        WALLJUMP = 5,
//        DROP = 6,
//        LAND = 7,
//        GIMMICKMOVE = 8,
//        GIMMICKDASH = 9,
//        DAMAGEHIT = 10,
//        DASH = 11,
//        PUSH = 12,
//        TALK = 13,
//        UseTimeSkill = 14,
//        Death = 15
//    }

//    protected override void InitStatePool()
//    {
//        base.InitStatePool();
//        StatePool[PlayerState.IDLE] = gameObject.AddComponent<InitState>(); // InitState -> PlayerIdleState ������ ���� �ʿ�
//        StatePool[PlayerState.MOVE] = gameObject.AddComponent<MoveState>(); // MoveState -> PlayerMoveState ������ ���� �ʿ�
//        StatePool[PlayerState.JUMP] = gameObject.AddComponent<JumpState>(); // JumpState -> PlayerJumpState ������ ���� �ʿ�
//        StatePool[PlayerState.WALL] = gameObject.AddComponent<WallState>(); // WallState -> PlayerWallState ������ ���� �ʿ�
//        StatePool[PlayerState.BOARD] = gameObject.AddComponent<BoardingState>(); // BoardingState -> PlayerBoardState ������ ���� �ʿ�
//        StatePool[PlayerState.WALLJUMP] = gameObject.AddComponent<WallJumpState>(); // WallJumpState -> PlayerWallJumpState ������ ���� �ʿ�
//        StatePool[PlayerState.DROP] = gameObject.AddComponent<DropState>(); // DropState -> PlayerDropState ������ ���� �ʿ�
//        StatePool[PlayerState.LAND] = gameObject.AddComponent<LandingState>(); // LandingState -> PlayerLandingState ������ ���� �ʿ�
//        StatePool[PlayerState.GIMMICKMOVE] = gameObject.AddComponent<GimmickJumpState>(); // GimmickJumpState -> PlayerGimmickMoveState ������ ���� �ʿ�
//        StatePool[PlayerState.GIMMICKDASH] = gameObject.AddComponent<GimmickDashState>(); // GimmickDashState -> PlayerGimmickDashState ������ ���� �ʿ�
//        StatePool[PlayerState.DASH] = gameObject.AddComponent<DashState>(); // DashState -> PlayerDashState ������ ���� �ʿ�
//        StatePool[PlayerState.DAMAGEHIT] = gameObject.AddComponent<DamageHitState>(); // DamageHitState -> PlayerDamageHitState ������ ���� �ʿ�
//        StatePool[PlayerState.PUSH] = gameObject.AddComponent<PushState>(); // PushState -> PlayerPushState ������ ���� �ʿ�
//        StatePool[PlayerState.TALK] = gameObject.AddComponent<TalkkState>(); // TalkkState -> PlayerTalkState ������ ���� �ʿ�
//        StatePool[PlayerState.UseTimeSkill] = gameObject.AddComponent<UseTimeSKillState>(); // UseTimeSKillState -> PlayerUseTimeSkillState ������ ���� �ʿ�
//        StatePool[PlayerState.Death] = gameObject.AddComponent<DeathState>(); // DeathState -> PlayerDeathState ������ ���� �ʿ�

//        foreach (var state in StatePool.Values)
//        {
//            if (state is IState<PlayerController> playerState)
//            {
//                playerState.Handle(GetComponent<PlayerController>());
//            }
//        }
//    }
//}
