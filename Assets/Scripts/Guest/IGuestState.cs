
// 상태 인터페이스
public interface IGuestState
{
    public void Enter(); // 상태 진입
    public void Exit(); // 상태 종료
    public void Execute(); // 상태 실행
}
