# Angles

![ezgif-3-d13b2d286e](https://github.com/user-attachments/assets/1c205434-7dd6-4cb2-b618-ed5f4a5f8bfc)
</br>
플레이 영상: https://www.youtube.com/watch?v=gC9k8V1H3kU

## 프로젝트 소개
Unity를 사용하여 개발한 캐주얼 2D 게임

## 개발 기간
24. 07 ~ 24. 08

## 인원
2명 (프로그래머 1명, 아트 1명)

## 개발 환경
Unity (C#)

## 기능 설명

### FSM을 활용하여 Player 기능 구현  

  <div align="center">
    <img src="https://github.com/user-attachments/assets/3d8a0394-e7f1-4673-ac25-cdaa950e8f9a" width="40%" height="40%"/>
  </div>

   <div align="center">
    <a href="https://github.com/minkimgyu/Angles/blob/7e1eb739d7b079452b59aaefde1a2fae9bbe6ef0/Angles2/Assets/Scripts/Player/Player.cs#L203">Player FSM 코드 보러가기</a>
  </div>
  
  </br>
  
   <div align="center">
    플레이어의 움직임, 슈팅 기능을 구현하기 위해 각각의 기능을 독립시켜 Concurrent State Machine을 적용했습니다.
  </div>



### Flock Algorithm를 활용한 AI 구현
  <div align="center">
    <img src="https://github.com/user-attachments/assets/05ebb2ec-d8aa-42d0-893c-fc400965218a" width="70%" height="70%"/>
    <img src="https://github.com/user-attachments/assets/388290ff-2805-4ce0-a67a-20418c08f1c6" width="60%" height="60%"/>
  </div>

   <div align="center">
    <a href="https://github.com/minkimgyu/Winter_Portfolio_Project/blob/c975441a8055f5e664d597710e416eef119e1bea/Winter_Portfolio_Project/Assets/Scripts/AI/Entity/Life/Unit/Unit.cs#L99">AttackUnit 코드 보러가기</a>
   </br>
    <a href="https://github.com/minkimgyu/Winter_Portfolio_Project/blob/c975441a8055f5e664d597710e416eef119e1bea/Winter_Portfolio_Project/Assets/Scripts/AI/Entity/Life/Building/Building.cs#L54C27-L54C42">AttackBuilding 코드 보러가기</a>
   </br>
    <a href="https://github.com/minkimgyu/Winter_Portfolio_Project/blob/c975441a8055f5e664d597710e416eef119e1bea/Winter_Portfolio_Project/Assets/Scripts/AI/Entity/Life/Building/Building.cs#L172">LiveOutSpawnBuilding 코드 보러가기</a>
  </div>
  </br>
AttackUnit, AttackBuilding, LiveOutSpawnBuilding에 Behavior Tree를 구현하여 공격, 생성 기능을 구현하여 유닛의 Base Class를 제작했습니다.

### Factory 패턴을 사용한 생성 시스템 개발
   <div align="center">
    <img src="https://github.com/minkimgyu/Winter_Portfolio_Project/assets/48249824/902c6289-c666-4228-a484-86473a3aa128" width="30%" height="30%"/>
    <img src="https://github.com/minkimgyu/Winter_Portfolio_Project/assets/48249824/8c24f618-dc03-49e5-9dfb-f50dcef90d6d" width="30%" height="30%"/>
  </div>

   <div align="center">
    <img src="https://github.com/user-attachments/assets/9038cf6d-76ed-4de5-9248-614217025ad5" width="60%" height="60%"/>
  </div>

  <div align="center">
    <a href="https://github.com/minkimgyu/Winter_Portfolio_Project/blob/9698e9124fb9c4698ac4604625f451922181f8bb/Winter_Portfolio_Project/Assets/Scripts/AI/Grid/GridController.cs#L12">GridController 코드 보러가기</a>
  </div>
  
  <div align="center">
    </br>
    GridController 내부에 FSM을 추가하여 Ready, Select, Plant State 기능을 구현했습니다.
  </div>

### MVC 패턴과 Observer 패턴을 활용하여 Skill, Card UI 개발
  <div align="center">
    <img src="https://github.com/minkimgyu/Winter_Portfolio_Project/assets/48249824/abac3128-2fa6-4cac-87ff-399256cd6b0a" width="60%" height="60%"/>
  </div>

  <div align="center">
    <a href="https://github.com/minkimgyu/Winter_Portfolio_Project/blob/c975441a8055f5e664d597710e416eef119e1bea/Winter_Portfolio_Project/Assets/Scripts/AI/Grid/PathFinder.cs#L29">Pathfinder 코드 보러가기</a>
  </div>
  </br>

 <div align="center">
   Pathfinder 내부에 A* 알고리즘을 구현하여 Unit에 길찾기 기능을 추가했습니다.
 </div>

### Addressable 사용하여 에셋 로드 시스템 개발
  <div align="center">
    <img src="https://github.com/minkimgyu/Winter_Portfolio_Project/assets/48249824/abac3128-2fa6-4cac-87ff-399256cd6b0a" width="60%" height="60%"/>
  </div>

  <div align="center">
    <a href="https://github.com/minkimgyu/Winter_Portfolio_Project/blob/c975441a8055f5e664d597710e416eef119e1bea/Winter_Portfolio_Project/Assets/Scripts/AI/Grid/PathFinder.cs#L29">Pathfinder 코드 보러가기</a>
  </div>
  </br>

 <div align="center">
   Pathfinder 내부에 A* 알고리즘을 구현하여 Unit에 길찾기 기능을 추가했습니다.
 </div>

### DotTween 에셋 활용하여 UI 애니메이션 적용
  <div align="center">
    <img src="https://github.com/minkimgyu/Winter_Portfolio_Project/assets/48249824/abac3128-2fa6-4cac-87ff-399256cd6b0a" width="60%" height="60%"/>
  </div>

  <div align="center">
    <a href="https://github.com/minkimgyu/Winter_Portfolio_Project/blob/c975441a8055f5e664d597710e416eef119e1bea/Winter_Portfolio_Project/Assets/Scripts/AI/Grid/PathFinder.cs#L29">Pathfinder 코드 보러가기</a>
  </div>
  </br>

 <div align="center">
   Pathfinder 내부에 A* 알고리즘을 구현하여 Unit에 길찾기 기능을 추가했습니다.
 </div>
 
## 회고
졸업 작품 프로젝트를 진행하면서 많은 부분을 새롭게 배우고 적용해봤습니다. 지금까지 했던 프로젝트 중에서 가장 많은 것을 얻어갈 수 있던 시간이었습니다.
