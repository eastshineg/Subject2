환경
=
unity 2022.3.52f

Summary
=
![image](https://github.com/user-attachments/assets/1351539d-8812-4aab-b4e1-b36cd81390e3)


과제
=

- **플레이**
  - Assets/Scene의 subject.scene을 open한다음 play
  - 과제 수행 진입점은 GameSupervisor의 Play()

- **GameSupervisor**
  - Singleton으로 작성
  - play 환경 기본 구축을 위한 시작점
  - item object는 effect 정보를 가지는 가상의 시나리오를 설정하고 이를 player object에 적용하는 로직을 구성
- **NeopleObject**
  - 서로 상호작용 및 제어를 위한 기본 class
  - **NeoplePlayerObject**
    - 회부에서 생성된 data형태의 effect정보를 받아서 effect를 실제로 적용하는 프로세스를 가지는 object 상세 class
    - **Stat**
      - hp, speed와 같은 데이터를 초기화 혹은 편화를 위한 처리를 일관성 있게 수행하기 위함.
      - IStat interface 통해 stat의 일관성 있게 필요한 부분을 정의해서 차후에 stat이 추가될때마다 stat의 초기화 및 변경에 대한 일관성 있는 함수를 제공해서 실제 세부 구현이 가능하게 처리
      - stat중에 reset통해 orignal값으로 변경되어야 하는 stat이 있고 일관되게 다른 값으로 변경되어야 하거나 reset을 통해 원래값을 복원하면 안되는 특성이 있기 때문에 IResetableStat 통해 reset 가능한 stat을 별개로 지정 및 복원처리가 가능하도록 세부 구현을 유도.
    - **EffectService**
      - effect의 효과를 stat에 실제 반영하기 위해 서로다른 세부 domain이 혼합되어 구현이 필요한 부분이 존재하기 때문에 service로 정의 후 세부구현 작성
      - 실제로 speed와 hp변경에 되한 세부 구현은 service로직에서 수행        - 
  - **NeopleItemObject**
    - Player object에 영향을 주기 위한 정보를 가지고 있는 상세 class
- **ObjectManager**
  - NeopleObject의 생성 및 파괴를 관리하는 클래스
- **IEffect**
  - Effect의 인터페이스
    - Effect는 다양한 형태로 생명주기를 가질 수 있기 때문에 abstract혹은 base 클래스로 구현하기보다는 최종적으로 결정된 정보를 반영하는 형태의 인터페이스가 더 적절한 형태로 구현
    - 즉발성도 있고, Update를 통해 시간 혹은 다수의 condition을가지고 효과적용을 다르게 할 수 있기 때문에 interface를 구현하여 확장하는 형태로 구현하는게 더 유연한 구조를 가질 수 있다고 판단되어 이를 상속받는 형태로 세부구현 진행
    - **IUpdateEffect**
      - update가 필요한 경우는 IUpdateEffect를 상속받도록 수정
      - effect가 update를 구현하는 상황도 피하고 이를 의미없이 호출하는 상황도 피할 수 있게 하는것이 목적(ex, - 즉발성 effect의 경우는 위와같은 update가 필요없음)
      
