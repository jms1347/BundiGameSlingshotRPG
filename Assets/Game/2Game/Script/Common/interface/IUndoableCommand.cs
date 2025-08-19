public interface IUndoableCommand
{
    void Execute(); // 명령 실행
    void Undo();    // 명령 취소
    void Redo();    // 명령 다시 실행
}
