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
5. Unity Hub → Add → Add project from disk → `C:\Dev\KK_GameProject` 선택 후 엽니다.
6. `KK_Temp` 폴더는 삭제해도 됩니다.

## 4. Unity 설정 (Git 협업용)

Edit → Project Settings → Editor:

- **Version Control → Mode**: Visible Meta Files
- **Asset Serialization → Mode**: Force Text

Edit → Preferences → External Tools → External Script Editor: Visual Studio 선택

## 5. 씬/프리팹 충돌 자동 병합 (Smart Merge)

Unity에 들어 있는 UnityYAMLMerge를 Git에 등록합니다. `<버전>` 부분은 실제 설치된 버전 폴더명으로 바꿔 주세요.

```
git config merge.unityyamlmerge.name "Unity SmartMerge"
git config merge.unityyamlmerge.driver "\"C:/Program Files/Unity/Hub/Editor/<버전>/Editor/Data/Tools/UnityYAMLMerge.exe\" merge -p %O %B %A %A"
git config merge.unityyamlmerge.recursive binary
```

## 6. 폴더 구조

내가 만든 파일은 전부 `Assets/_Project` 아래에 둡니다. 외부 에셋과 섞이지 않게 하기 위해서입니다.

```
Assets/
  _Project/
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

## 7. 첫 커밋

```
git checkout -b feature/unity-project-init
git add .
git commit -m "feat: Unity 6 URP 프로젝트 생성"
git push -u origin feature/unity-project-init
```

GitHub에서 Pull Request를 만들어 `main`에 합칩니다.

## 브랜치 / 커밋 규칙

- `main`: 항상 실행 가능한 상태. 직접 push하지 않습니다.
- 작업은 `feature/기능명` 브랜치에서 하고 PR로 합칩니다.
- 커밋 메시지 접두어: `feat:` 기능, `fix:` 버그, `refactor:` 구조 개선, `docs:` 문서, `chore:` 설정
- 메인 씬(`Main.unity`)은 한 번에 한 사람만 수정합니다. 실험은 개인 Sandbox 씬에서 합니다.
