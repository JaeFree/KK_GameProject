# KK_GameProject

Unity 3D로 만드는 2인 협동 탑다운 생존 액션 게임입니다.

- 엔진: Unity 6 LTS (URP), C#
- 플랫폼: Windows PC
- 팀: 조교님(기획), 김재표(개발)

## 시작하기

- 개발 환경 세팅: [docs/SETUP.md](docs/SETUP.md)
- 1주차 과제: [docs/WEEK1.md](docs/WEEK1.md)

## 기본 코드 (장르와 상관없이 쓰는 것)

| 파일 | 역할 |
| --- | --- |
| `Scripts/Core/GameManager.cs` | 게임 상태(준비, 플레이, 일시정지, 패배, 승리) 관리 |
| `Scripts/Core/GameEvents.cs` | 스크립트끼리 이벤트로 소식 전달 (죽음, 피격) |
| `Scripts/Combat/IDamageable.cs` | "데미지를 받을 수 있는 것" 공통 인터페이스 |
| `Scripts/Combat/Health.cs` | 체력, 피격, 회복, 죽음 처리 (플레이어와 적이 같이 사용) |
| `Scripts/Systems/SceneLoader.cs` | 씬 이동, 재시작, 종료 |
| `Editor/ProjectSetup.cs` | 메뉴 `KK → 프로젝트 기본 설정 적용` |
| `Editor/GitWorkflowReminder.cs` | 에디터 시작 시 브랜치와 pull 안내 |

경로는 모두 `Assets/_Project/` 아래입니다.
