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
//        StatePool[PlayerState.IDLE] = gameObject.AddComponent<InitState>(); // InitState -> PlayerIdleState 등으로 변경 필요
//        StatePool[PlayerState.MOVE] = gameObject.AddComponent<MoveState>(); // MoveState -> PlayerMoveState 등으로 변경 필요
//        StatePool[PlayerState.JUMP] = gameObject.AddComponent<JumpState>(); // JumpState -> PlayerJumpState 등으로 변경 필요
//        StatePool[PlayerState.WALL] = gameObject.AddComponent<WallState>(); // WallState -> PlayerWallState 등으로 변경 필요
//        StatePool[PlayerState.BOARD] = gameObject.AddComponent<BoardingState>(); // BoardingState -> PlayerBoardState 등으로 변경 필요
//        StatePool[PlayerState.WALLJUMP] = gameObject.AddComponent<WallJumpState>(); // WallJumpState -> PlayerWallJumpState 등으로 변경 필요
//        StatePool[PlayerState.DROP] = gameObject.AddComponent<DropState>(); // DropState -> PlayerDropState 등으로 변경 필요
//        StatePool[PlayerState.LAND] = gameObject.AddComponent<LandingState>(); // LandingState -> PlayerLandingState 등으로 변경 필요
//        StatePool[PlayerState.GIMMICKMOVE] = gameObject.AddComponent<GimmickJumpState>(); // GimmickJumpState -> PlayerGimmickMoveState 등으로 변경 필요
//        StatePool[PlayerState.GIMMICKDASH] = gameObject.AddComponent<GimmickDashState>(); // GimmickDashState -> PlayerGimmickDashState 등으로 변경 필요
//        StatePool[PlayerState.DASH] = gameObject.AddComponent<DashState>(); // DashState -> PlayerDashState 등으로 변경 필요
//        StatePool[PlayerState.DAMAGEHIT] = gameObject.AddComponent<DamageHitState>(); // DamageHitState -> PlayerDamageHitState 등으로 변경 필요
//        StatePool[PlayerState.PUSH] = gameObject.AddComponent<PushState>(); // PushState -> PlayerPushState 등으로 변경 필요
//        StatePool[PlayerState.TALK] = gameObject.AddComponent<TalkkState>(); // TalkkState -> PlayerTalkState 등으로 변경 필요
//        StatePool[PlayerState.UseTimeSkill] = gameObject.AddComponent<UseTimeSKillState>(); // UseTimeSKillState -> PlayerUseTimeSkillState 등으로 변경 필요
//        StatePool[PlayerState.Death] = gameObject.AddComponent<DeathState>(); // DeathState -> PlayerDeathState 등으로 변경 필요

//        foreach (var state in StatePool.Values)
//        {
//            if (state is IState<PlayerController> playerState)
//            {
//                playerState.Handle(GetComponent<PlayerController>());
//            }
//        }
//    }
//}
