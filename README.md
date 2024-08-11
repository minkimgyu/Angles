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

### Flocking Algorithm를 활용한 Enemy AI 구현
  <div align="center">
    <img src="https://github.com/user-attachments/assets/28c82035-7b42-4b73-a3f5-03c8100c37a8" width="40%" height="40%"/>
  </div>

   <div align="center">
    <a href="https://github.com/minkimgyu/Angles/tree/main/Angles2/Assets/Scripts/Enemy/Component/Flock/Behavior">Behavior 코드 보러가기</a>
    </br>
    <a href="https://github.com/minkimgyu/Angles/blob/d24868f5af10776a53480baafc25e3bd9e0c78d8/Angles2/Assets/Scripts/Enemy/BaseEnemy.cs#L87">Enemy 코드 보러가기</a>
  </div>
  </br>

* Flocking Algorithm을 활용하여 기본적인 움직임을 구현하기 위해 Alignment, Cohesion, Separation Behavior를 개발했습니다.
* 추적과 회피 기능을 구현하기 위해 Follow, Avoid Behavior를 개발하여 적용했습니다.


### Factory 패턴을 사용한 생성 시스템 개발
  <div align="center">
    <img src="https://github.com/user-attachments/assets/c19bae57-8c80-482e-9bf5-ed52eaa0d657" width="60%" height="60%"/>
  </div>
  <div align="center">
    <a href="https://github.com/minkimgyu/Angles/blob/5db5c73fec6d43f2bbef57e8db8e99c0a35a4d0a/Angles2/Assets/Scripts/Factory/FactoryCollection.cs#L15">Factory 코드 보러가기</a>
  </div>
  
  <div align="center">
    </br>
    Factory 패턴을 적용하여 객체를 생성하는 시스템을 분리해서 개발했습니다.
  </div>

### MVC 패턴과 Observer 패턴을 활용하여 Card UI 개발
  <div align="center">
    <img src="https://github.com/user-attachments/assets/10977106-5595-40dd-850d-8fe411dd8ce1" width="60%" height="60%"/>
  </div>

  <div align="center">
    <a href="https://github.com/minkimgyu/Angles/blob/c57fee5b75addeed51b7f2a095265613e0eb14fc/Angles2/Assets/Scripts/Card/CardViewer.cs#L9">CardViewer 코드 보러가기</a>
    </br>
    <a href="https://github.com/minkimgyu/Angles/blob/c57fee5b75addeed51b7f2a095265613e0eb14fc/Angles2/Assets/Scripts/Card/CardUIController.cs#L10">SkillCardData 코드 보러가기</a>
    </br>
    <a href="https://github.com/minkimgyu/Angles/blob/c57fee5b75addeed51b7f2a095265613e0eb14fc/Angles2/Assets/Scripts/Card/CardUIController.cs#L52">CardUIController 코드 보러가기</a>
    </br>
  </div>
  </br>

 <div align="center">
   MVC, Observer 패턴을 적용하여 결합도를 낮추어 코드의 재사용성을 높혔습니다.
 </div>

### Addressable 사용하여 에셋 로드 시스템 개발
  <div align="center">
    <img src="https://github.com/user-attachments/assets/36949272-e945-4ef1-bde0-88abf2137a0d" width="70%" height="70%"/>
  </div>

  <div align="center">
    <a href="https://github.com/minkimgyu/Angles/blob/0ecc75af92c25b288bed8dc2d4138601570dc52a/Angles2/Assets/Scripts/AddressableHandler.cs#L10">AddressableHandler 코드 보러가기</a>
  </div>
  </br>

 <div align="center">
   Addressable를 활용하여 에셋을 로드하는 기능을 개발했습니다.
 </div>

### DotTween 에셋 활용하여 UI 애니메이션 적용
  <div align="center">
    <img src="https://github.com/user-attachments/assets/afab2d8f-6749-41ef-9ec8-056b37da60ca" width="60%" height="60%"/>
  </div>

  <div align="center">
    <a href="https://github.com/minkimgyu/Angles/blob/a97cf9e1f6d963d1f5a27f5047298a03a89803cf/Angles2/Assets/Scripts/UI/Viewer/GameEndViewer.cs#L28">코드 보러가기</a>
  </div>
  </br>

 <div align="center">
   DotTween 에셋을 활용하여 UI 애니메이션을 제작해봤습니다.
 </div>
 
## 회고
졸업 작품 프로젝트를 진행하면서 많은 부분을 새롭게 배우고 적용해봤습니다. 지금까지 했던 프로젝트 중에서 가장 많은 것을 얻어갈 수 있던 시간이었습니다.
