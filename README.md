# 🎮 Angles

Unity를 사용하여 개발한 캐주얼 모바일 2D 게임입니다.

스킬을 획득하고 적을 쓰러트려 모든 스테이지를 클리어하는 것이 목표입니다.

[Google Play Store](https://play.google.com/store/apps/details?id=com.hatchling.angles&hl=ko)에서 서비스 진행 중입니다.

<img src="https://github.com/user-attachments/assets/64297dd4-b1d5-4ed6-98d4-e7b84679bcd5" alt="Angles Game Screenshot"/>


## 📆 개발 기간
2024년 11월 ~ 2025년 2월

## 🧑‍🤝‍🧑 팀 구성
- 총 2명
  - 클라이언트 프로그래머 1명
  - 아트 디자이너 1명

## 🛠️ 개발 도구
- Unity (C#)

## 👨‍💻 담당 역할 및 기여도 (기여도 100%)

- ✅ **Finite State Machine을 활용하여 Player 기능 구현**
- ✅ **A\* 길찾기 알고리즘을 활용한 챕터 모드 전용 퍼즐 시스템 개발 및 적용**
- ✅ **서바이벌 모드 최적화를 위한 JPS+ 길찾기 알고리즘 개발 및 적용**
- ✅ **Strategy Pattern을 활용한 무기, 스킬 시스템 개발**
- ✅ **Visitor Pattern을 활용한 업그레이드 시스템 개발**
- ✅ **MVC 패턴을 활용한 StageUIController 개발** 
- ✅ **커스텀 에디터를 활용한 스테이지 에디터 개발**
- ✅ **Google Play Store 출시를 위한 인앱 업데이트, 클라우드 저장, 구글 로그인 적용**
- ✅ **수익 창출을 위한 구글 애드몹 광고 적용**

---

## 🏃 Finite State Machine (FSM)을 활용하여 Player 기능 구현

플레이어의 움직임, 슈팅 기능을 구현하기 위해 각각의 기능을 독립시켜 **Concurrent State Machine**을 적용했습니다.
이를 통해, 새로운 기능 추가 시 사전 코드의 수정 없이 유연하게 확장이 가능한 구조를 구축했습니다.

[FSM 구현 코드](https://github.com/minkimgyu/Angles/blob/c839bd712e3c24aab24a4ba1bf97571933b9c801/AnglesTest/Assets/Scripts/Player/Player.cs#L193)

[Stop State 구현 코드](https://github.com/minkimgyu/Angles/blob/c839bd712e3c24aab24a4ba1bf97571933b9c801/AnglesTest/Assets/Scripts/Player/FSM/MovementState/StopState.cs#L6)
[Move State 구현 코드](https://github.com/minkimgyu/Angles/blob/c839bd712e3c24aab24a4ba1bf97571933b9c801/AnglesTest/Assets/Scripts/Player/FSM/MovementState/MoveState.cs#L6)
[Dash State 구현 코드](https://github.com/minkimgyu/Angles/blob/c839bd712e3c24aab24a4ba1bf97571933b9c801/AnglesTest/Assets/Scripts/Player/FSM/MovementState/DashState.cs#L6)

[Ready State 구현 코드](https://github.com/minkimgyu/Angles/blob/c839bd712e3c24aab24a4ba1bf97571933b9c801/AnglesTest/Assets/Scripts/Player/FSM/ActionState/ReadyState.cs#L6)
[Charge State 구현 코드](https://github.com/minkimgyu/Angles/blob/c839bd712e3c24aab24a4ba1bf97571933b9c801/AnglesTest/Assets/Scripts/Player/FSM/ActionState/ChargeState.cs#L6)
[Shoot State 구현 코드](https://github.com/minkimgyu/Angles/blob/c839bd712e3c24aab24a4ba1bf97571933b9c801/AnglesTest/Assets/Scripts/Player/FSM/ActionState/ShootState.cs#L6)



### FSM 다이어그램 📊

<img src="https://github.com/user-attachments/assets/2c82888a-4a0e-4ee0-ad96-7057a2baf830" alt="Angles Game Screenshot"/>

---

## 🔍 챕터 모드 전용 A* 길찾기 알고리즘 개발 및 적용

Tilemap 정보를 기반으로 Grid를 생성하고, A* 알고리즘을 활용하여 길찾기 기능을 구현했습니다.
유닛 크기에 따라 알맞은 경로를 찾아냄으로써, 게임 내 유닛의 자연스러운 이동이 가능하게 했습니다.

[AStar 구현 코드](https://github.com/minkimgyu/Angles/blob/4f6b993bd5160e606e8a547aee7dd6446f39f026/AnglesTest/Assets/Scripts/AStar/Pathfinder.cs#L8)

### 길찾기 예시 📏

<img src="https://github.com/user-attachments/assets/0733f9c7-43f2-4075-8c71-56329dbeb17e" alt="Angles Game Screenshot" width="85%" height="85%" />

*Tilemap 기반 A\* 길찾기 시각화*

---

## 🛠️ 챕터 모드 전용 A* 길찾기 알고리즘 성능 문제점 해결

### A* 알고리즘 비효율적 탐색 문제 발생 ⚠️

* 테스트 플레이 중 10-20개 이상의 플레이어가 존재할 때 프레임 드랍 현상을 발견했습니다.
* **과도하게 노드를 탐색하는 문제:** 분석 결과, 휴리스틱(h_cost)을 업데이트하는 로직의 버그로 인해 휴리스틱 값이 반영되지 않아 A* 알고리즘이 과도하게 많은 노드를 탐색하는 것이 확인되었습니다.

### 개선 전후 A* 탐색 비교 📈

<img src="https://github.com/user-attachments/assets/dde2ae75-febd-4b53-b104-bf471bd482b0" alt="Angles Game Screenshot" />

*좌: 잘못된 경우 (비효율적 탐색), 우: 정상적인 경우 (효율적 탐색)*

### 개선 후 Deep Profiling 결과 ⚡

<img src="https://github.com/user-attachments/assets/0b50d2eb-ce43-4c0c-bf8f-ca79895fc19f" alt="Angles Game Screenshot"  />

최적화 후 A\* 연산 시간이 17.35ms에서 **4.8ms로** 단축되어 약 **3.61배의 성능 향상**을 달성하며 프레임 드랍 문제를 해결했습니다.

---

## 🚀 서바이벌 모드 최적화를 위한 JPS+ 길찾기 알고리즘 개발

최대 100개의 객체가 플레이어를 동시에 추적하는 서바이벌 모드 특성상, 연산 부하를 줄이기 위해 기존 A* 알고리즘보다 더 효율적인 길찾기 방식이 필요했습니다.

<img src="https://github.com/user-attachments/assets/62fc1d6c-34a7-4c6d-a341-f7e5923f0c50" alt="Angles Game Screenshot" />

### 성능 목표 🎯

* **60fps 기준:** 1프레임당 허용 가능한 수행 시간은 약 16.67ms입니다.
* **컴포넌트별 제한:** Rendering, Physics, Others, UI 수행 시간을 고려하면 Scripts는 최대 5ms 이내로 제한되어야 합니다.
* **길찾기 최적화:** 최악의 경우에도 길찾기 알고리즘의 수행 시간이 5ms의 10%를 넘지 않도록 최적화합니다.

### 평균적인 수행 상황 프로파일링 결과 📊

<img src="https://github.com/user-attachments/assets/996a08df-1c72-4759-9f2f-44034b0576fc" alt="Angles Game Screenshot" />

*평균적인 수행 상황 프로파일링 결과 (좌: 게임 화면, 우: Profiler 데이터)*

### 진행 방식 💡

*플레이 도중 기존 A\* 알고리즘과 JPS+ 간 평균 및 최악의 경우를 비교하여 더 효율적인 알고리즘을 선택했습니다.*

### 📊 성능 비교 프로파일링 결과 (A* vs JPS+)

### 평균적인 경우 📈

<img src="https://github.com/user-attachments/assets/ed85b2bd-a7a6-46db-8861-90b396dc1fb7" alt="Angles Game Screenshot"/>

*좌: A\* 알고리즘 (수행 시간: 4.46ms, 요구 수행 시간 대비 비율: 89.2%)*

*우: JPS+ 알고리즘 (수행 시간: 0.25ms, 요구 수행 시간 대비 비율: 5%)*

### 최악의 경우 📉

<img src="https://github.com/user-attachments/assets/d4f7bf7e-f227-4c8d-8174-b2f7fd6c956e" alt="Angles Game Screenshot" />

*좌: A\* 알고리즘 (수행 시간: 7.75ms, 요구 수행 시간 대비 비율: 155%)*

*우: JPS+ 알고리즘 (수행 시간: 0.41ms, 요구 수행 시간 대비 비율: 8.2%)*

### 💡 결론: JPS+ 알고리즘 적용

[JPS+ 구현 코드](https://github.com/minkimgyu/Angles/blob/4f6b993bd5160e606e8a547aee7dd6446f39f026/AnglesTest/Assets/Scripts/JPSPlus/FastJPSPlus.cs#L6)

### A* 알고리즘의 한계 🚫

A* 알고리즘은 경로를 찾아가는 과정에서 탐색하는 노드의 수가 증가하며, 특히 거리가 멀어질수록 연산량 및 메모리 사용량이 급격히 증가하여 탐색 시간의 지연과 프레임 드랍을 유발했습니다.

### JPS+ 알고리즘의 장점 ✨

JPS+는 사전에 계산된 점프 포인트를 활용하고, 불필요한 노드의 탐색을 줄이는 가지치기 규칙을 적용하여 탐색 경로를 제한함으로써 **안정적이고 예측 가능한 성능**을 제공합니다.

### JPS+ 적용 결과 ✅

JPS+ 알고리즘을 적용해 요구 성능을 안정적으로 달성함으로써, 실시간 처리가 중요한 게임 환경에서도 프레임 안정성을 확보할 수 있었습니다.

### A* vs JPS+ 탐색 과정 시각화 🖼️

<img src="https://github.com/user-attachments/assets/64d790df-e6d8-405d-9638-9cc4369d2126" alt="Angles Game Screenshot"/>

*좌: A\* 알고리즘 탐색 과정, 우: JPS+ 알고리즘 탐색 과정*

[참고한 깃허브 레포지토리](https://github.com/trgrote/JPS-Unity)

---

## 🛡️ Strategy Pattern을 활용한 무기, 스킬 시스템 개발

다양한 무기 작동 방식과 효과 알고리즘을 전략 클래스로 분리하여, 기존 코드 수정 없이 새로운 기능을 쉽게 추가하고 유연하게 교체할 수 있는 확장성 높은 구조를 구축했습니다.

[BaseWeapon 구현 코드](https://github.com/minkimgyu/Angles/blob/4f6b993bd5160e606e8a547aee7dd6446f39f026/AnglesTest/Assets/Scripts/Weapon/BaseWeapon.cs#L27)
[BaseSkill 구현 코드](https://github.com/minkimgyu/Angles/blob/4f6b993bd5160e606e8a547aee7dd6446f39f026/AnglesTest/Assets/Scripts/Skill/BaseSkill.cs#L63)

### 스킬 클래스 구조 📜

<img src="https://github.com/user-attachments/assets/464272b2-954c-494a-a193-0349561d3469" alt="Angles Game Screenshot" />

---

## 🎨 커스텀 에디터를 활용한 스테이지 에디터 개발

각 스테이지별 효율적이고 직관적인 적 배치를 위해 자체적인 커스텀 스테이지 에디터를 개발했습니다.
개발된 에디터는 GitHub Release를 활용하여 팀원에게 배포하고 있습니다.
이러한 과정을 통해 스테이지 레벨 디자인에 대한 팀 간의 협업 효율성을 향상시켰습니다.

### GitHub Release를 통한 배포 📤

<img src="https://github.com/user-attachments/assets/114b5329-6db6-4fcf-80b2-65d6e1af9fa9" alt="GitHub Release" />

*GitHub Release 페이지 (v1.1.7)*

### 커스텀 스테이지 에디터 🕹️

<img src="https://github.com/user-attachments/assets/352f8857-fae8-42b7-92e7-40eb0ae1ec0a" alt="Custom Stage Editor" />

*커스텀 스테이지 에디터 UI 및 레벨 편집 화면*

**GitHub Repository:** [https://github.com/minkimgyu/AnglesStageDesigner](https://github.com/minkimgyu/AnglesStageDesigner)
