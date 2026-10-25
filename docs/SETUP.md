# 개발 환경 세팅 (집 PC)

처음 한 번만 하면 됩니다. 순서대로 따라 하고, 막히면 에러 메시지나 스크린샷을 그대로 보내 주세요.

## 1. 설치

1. [Unity Hub](https://unity.com/download) 설치 후 로그인 (개인 무료 라이선스)
2. Unity Hub → Installs → Install Editor → **Unity 6 LTS** (6000.0.x 중 최신) 설치
   - 모듈: **Microsoft Visual Studio Community**, **Windows Build Support (IL2CPP)** 체크
3. [Git for Windows](https://git-scm.com/download/win) 설치 (설치 옵션은 기본값)
4. Git LFS 활성화: 명령 프롬프트(cmd)에서

   ```
   git lfs install
   ```

5. (선택) GitHub Desktop: 명령어가 불편하면 같이 써도 됩니다. 조교님께는 이걸 권장합니다.

## 2. 저장소 받기

```
cd C:\Dev
git clone https://github.com/JaeFree/KK_GameProject.git
```

경로에 한글과 공백이 없는 곳(예: `C:\Dev`)을 권장합니다. Unity가 한글 경로에서 가끔 문제를 일으킵니다.

## 3. Unity 프로젝트 만들기 (한 번만, 김재표)

Unity Hub는 이미 파일이 있는 폴더에 새 프로젝트를 바로 만들지 못하므로, 다른 곳에 만든 뒤 옮깁니다.

1. Unity Hub → New project → 템플릿 **Universal 3D** (URP) 선택
2. 이름 `KK_Temp`, 위치 `C:\Dev` → Create project
3. 프로젝트가 열리면 Unity를 닫습니다.
4. `C:\Dev\KK_Temp` 안의 **`Assets`, `Packages`, `ProjectSettings`** 세 폴더만 `C:\Dev\KK_GameProject`로 복사합니다. (`Library`는 복사하지 않아도 됩니다)
   - 저장소에 이미 `Assets` 폴더가 있으니 "덮어쓰기/병합"을 선택하면 됩니다. 기존 `Assets/_Project/Editor` 파일은 그대로 둡니다.
5. Unity Hub → Add → Add project from disk → `C:\Dev\KK_GameProject` 선택 후 엽니다.
6. `KK_Temp` 폴더는 삭제해도 됩니다.

## 4. Unity 기본 설정 (메뉴 한 번)

프로젝트가 열리면 상단 메뉴 **KK → 프로젝트 기본 설정 적용**을 누릅니다. 아래가 자동으로 처리됩니다.

- Asset Serialization: Force Text, Version Control: Visible Meta Files (Git 협업용)
- Company / Product Name: KK / KK_GameProject
- 빠진 폴더 생성
- `Main` 씬과 개인 실험용 `Sandbox_JaeFree` 씬 생성 (바닥, 탑다운 카메라, GameManager 포함)
- `Main` 씬을 빌드 씬 목록 맨 앞에 추가

URP 템플릿이 만든 `Assets/Scenes/SampleScene`, `Assets/Readme` 같은 예제 파일은 지워도 됩니다. 끝나면 Edit → Project Settings → Editor에서 두 설정이 바뀌었는지 눈으로 한 번 확인합니다.

Edit → Preferences → External Tools → External Script Editor: Visual Studio 선택

## 5. Git 작업 안내와 main 커밋 차단 켜기

이 저장소에는 두 가지 안내 장치가 있습니다.

- **Unity 안내 창**: Unity 에디터를 열 때마다 현재 브랜치를 보여 주고 "작업 전에 pull 하세요"를 띄웁니다. `main`에 있으면 개별 브랜치를 만들라고 경고합니다. 메뉴 `KK > Git 작업 안내 보기`로 다시 열 수 있습니다.
- **Git hook** (`.githooks/`): `main`에 직접 커밋하면 차단하고, 브랜치를 바꿀 때마다 pull 안내를 출력합니다.

Git hook은 Unity를 한 번 열면 자동으로 등록됩니다. Unity 없이 쓰는 PC(조교님 등)에서는 저장소 폴더에서 한 번만 실행해 주세요.

```
git config core.hooksPath .githooks
```

## 6. 씬/프리팹 충돌 자동 병합 (Smart Merge)

Unity에 들어 있는 UnityYAMLMerge를 Git에 등록합니다. `<버전>` 부분은 실제 설치된 버전 폴더명으로 바꿔 주세요.

```
git config merge.unityyamlmerge.name "Unity SmartMerge"
git config merge.unityyamlmerge.driver "\"C:/Program Files/Unity/Hub/Editor/<버전>/Editor/Data/Tools/UnityYAMLMerge.exe\" merge -p %O %B %A %A"
git config merge.unityyamlmerge.recursive binary
```

## 7. 폴더 구조

내가 만든 파일은 전부 `Assets/_Project` 아래에 둡니다. 외부 에셋과 섞이지 않게 하기 위해서입니다.

```
Assets/
  _Project/
    Editor/       에디터 전용 도구 (Git 작업 안내 창)
    Scripts/
      Player/     이동, 입력, 스킬
      Combat/     데미지, 피격, 투사체
      Enemy/      적 AI, 스폰
      Systems/    게임 흐름, 레벨업, 오브젝트 풀
      UI/
    Data/         ScriptableObject 수치 (조교님이 수정하는 곳)
    Prefabs/
    Scenes/
      Main.unity
      Sandbox_JaeFree.unity   개인 실험용
    Art/
    Audio/
  ThirdParty/     무료 에셋 (Kenney, Quaternius 등)
```

## 8. 첫 커밋

```
git checkout -b feature/unity-project-init
git add .
git commit -m "feat: Unity 6 URP 프로젝트 생성"
git push -u origin feature/unity-project-init
```

GitHub에서 Pull Request를 만들어 `main`에 합칩니다.

## 작업 시작 루틴 (매번)

작업을 시작할 때마다 이 순서를 지킵니다. 두 사람이 같은 파일을 다른 버전에서 고치는 충돌을 막는 가장 확실한 방법입니다.

```
git switch main
git pull
git switch -c feature/작업이름     # 이어서 하던 작업이면: git switch feature/작업이름 && git merge main
```

작업이 끝나면:

```
git add .
git commit -m "feat: 무엇을 했는지"
git push -u origin feature/작업이름
```

그다음 GitHub에서 PR을 만들고, 상대가 확인한 뒤 `main`에 merge 합니다. merge된 브랜치는 지웁니다.

## 브랜치 / 커밋 규칙

- `main`: 항상 실행 가능한 상태. 직접 커밋·push하지 않습니다. (Git hook이 커밋을 막습니다)
- 작업은 사람·작업마다 개별 브랜치에서 합니다: `feature/기능명`, 버그는 `fix/버그명`
- 한 브랜치는 2~3일 안에 PR로 합칩니다. 오래 두면 충돌이 커집니다.
- 커밋 메시지 접두어: `feat:` 기능, `fix:` 버그, `refactor:` 구조 개선, `docs:` 문서, `chore:` 설정
- 메인 씬(`Main.unity`)은 한 번에 한 사람만 수정합니다. 실험은 개인 Sandbox 씬에서 합니다.
