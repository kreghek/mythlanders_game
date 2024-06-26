﻿---
root:
  paragraphs:
    - text: |
        Несколько лет назад на далекой планете-мире Фронтир начали возраждаться древние цивилизации.
        Люди, ведомые искусственным интеллектом, быстро погружались в новый мир.
        Античные эпохи странным и удивительным образом переплеталось с современным технологичным укладом.
        Этот мир постепенно поделился на части, где господствовала одна из древних культур.
      env:
        - type: Background
          data: AncientRising
        - type: PlayMusic
          data: Battle
  options:
    - text: 'Далее'
      next: p2

p2:
  paragraphs:
    - text: |
        Но вслед за древними обычаями и великолепными храмами в мире стали появляться отвратительные монстры из сказок и легенд прошлого.
      env:
        - type: Background
          data: Monsters
  options:
    - text: 'Монстры были сильными'
      next: p3
      aftermaths:
        - type: AddMonsterPerk
          data: ExtraHitPoints
        - type: AddMonsterPerk
          data: ImprovedMeleeDamage
        - type: AddMonsterPerk
          data: LastBreath
      description: |
        В свободной игре некоторые монстры будут обладать следующими усилениями:
        - Экстра здоровье (+1ед.)
        - Улучшенный урон рукопашных атак (+1ед.)
    - text: 'Монстры были умными'
      next: p3
      aftermaths:
        - type: AddMonsterPerk
          data: ExtraShieldPoints
        - type: AddMonsterPerk
          data: ImprovedRangeDamage
        - type: AddMonsterPerk
          data: RearguardReduceHitPoints
      description: |
         В свободной игре некоторые монстры будут обладать следующими усилениями:
         - Экстра щиты (+1ед.)
         - Улучшенный урон стрелковых атак (+1ед.)
    - text: 'Монстры были сплоченными'
      next: p3
      aftermaths:
        - type: AddMonsterPerk
          data: VanguardExtraHitPoints
        - type: AddMonsterPerk
          data: VanguardReduceHitPoints
        - type: AddMonsterPerk
          data: ImprovedAllDamage
      description: |
         В свободной игре некоторые монстры будут обладать следующими усилениями:
         - Экстра здоровье на передовой (+1ед.)
         - Улучшенный урон всех атак (+1ед.)

p3:
  paragraphs:
    - text: |
        Никто уже давно не верил в мистику и нечистую силу.
        Люди будущего оказались совершенно не готовы к тому, чтобы выжить в таком мире.
        Иммунитет, помогавший раньше противостоять нечисти, был давно утерян.
      env:
        - type: Background
          data: MonstersAttack
  options:
    - text: 'Далее'
      next: p4

p4:
  paragraphs:
    - text: |
        К счастью, вслед за монстрами, в мир пришли герои былых времен.
        Их было не много. Но каждый из них обладал нечеловеческой силой.
        Секрет всему - Кровь Героев!
      env:
        - type: Background
          data: Hero
  options:
    - text: 'Далее'
      next: p5

p5:
  paragraphs:
    - text: |
        За Кровью Героев - этим ценнейшим ресурсом, началась охота со стороны господствующих в будущем фракций.
        Религиозный Черный Конклав, почитающий бессмертие, отправил на Фронтир своих лучших офицеров, презирающих биологическую жизнь, как слабость и низшую форму.
        А следом и Союз Промышленников, монополизирующий добычу тяжелых металлов, выслал своих безликих наемников, одержимых жаждой материальный наживы.
        Кто из них оккупировал Фронтир первым?
      env:
        - type: Background
          data: FirstFraction
  options:
    - text: 'Черный Конклав'
      next: p6_black
      aftermaths:
        - type: AddMonsterPerk
          data: BlackMessiah
        - type: AddMonsterPerk
          data: UnitedRush
      description: |
        В свободной игре некоторые элитные юниты Черных могут обладать способностью Черный Мессия:
        - Защита от стрелкового урона (+2ед.)
        - Решимость тыла противника снижена (+1ед.)
        В свободной игре некоторые элитные юниты Союза могут обладать способностью Союзный Раш:
        - Решимость своего авангарда увеличена (+2ед.)
    - text: 'Союз Промышленников'
      next: p6_mining
      aftermaths:
        - type: AddMonsterPerk
          data: UnitedTactics
        - type: AddMonsterPerk
          data: DefenderOfFaith
      description: |
        В разработке

        В свободной игре некоторые элитные юниты Союза могут обладать способностью Тактика Единения:
        - Улучшенный урон стрелковых атак (+2ед.)
        - Второе дыхание своему авангарду
        В свободной игре некоторые элитные юниты Черных могут обладать способностью Поборник Веры:
        - Щит всех дружественных юнитов увеличен (+2ед.)

p6_black:
  paragraphs:
    - text: |
        Тео-инженеры Черного Конклава соорудили оборонительные редуты. А полу-мертвые аристократы уже начали охоту на героев в тот момент,
        когда многочисленные наемники Союза Промышленников десантировались на поверхность планеты.
        Чтобы сломить оборону Черных, Промышленники воспользовались численным преимуществом.
        Планета была изолирована, как требуют современные правила ведения вооруженных конфликтов.
        Теперь никто не мог покинуть Фронтир до капитуляции одной из сторон.
        Как и весь остальной мир не знал, что твориться на Фронтире.
      env:
        - type: Background
          data: Black
  options:
    - text: 'Далее'
      next: p7

p6_mining:
  paragraphs:
    - text: |
        Бессчетные группы боевиков Союза Промышленников уже наладили логистику и начали охоту на героев в тот момент,
        когда элитное звено Черного Конклава десантировалось на поверхность планеты.
        Чтобы получить тактическое превосходство Промышленников, Черные решили использовать секретное оружие - Поборник Веры.
        Планета была изолирована, как требуют современные правила ведения вооруженных конфликтов.
        Теперь никто не мог покинуть Фронтир до капитуляции одной из сторон.
        Как и весь остальной мир не знал, что твориться на Фронтире.
      env:
        - type: Background
          data: Union
  options:
    - text: 'Далее'
      next: p7

p7:
  paragraphs:
    - text: |
        Древние герои были обречены. Однако, один из них получил знак. Что за герой это был?
      env:
        - type: Background
          data: StartHeroes
  options:
    - text: 'Монах'
      next: p8_monk
      aftermaths:
        - type: AddHero
          data: Monk
        - type: UnlockLocation
          data: Monastery
      description: |
        В разработке
    - text: 'Ратник'
      next: p8_swordsman
      aftermaths:
        - type: AddHero
          data: Swordsman
        - type: UnlockLocation
          data: Thicket
    - text: 'Спартанец'
      next: p8_spartian
      aftermaths:
        - type: AddHero
          data: Hoplite
        - type: UnlockLocation
          data: ShipGraveyard
      description: |
        В разработке
    - text: 'Освободительница'
      next: p8_liberator
      aftermaths:
        - type: AddHero
          data: Liberator
        - type: UnlockLocation
          data: Desert
      description: |
        В разработке

p8_monk:
  paragraphs:
    - text: |
        Шаолиньский монах Маосин начал свое приключение.
      env:
        - type: Background
          data: Monk

p8_swordsman:
  paragraphs:
    - text: |
        Славянский богатырь Беримир начал свое приключение.
      env:
        - type: Background
          data: Swordsman

p8_spartian:
  paragraphs:
    - text: |
        Спартанский гоплит Леонидас начал свое приключение.
      env:
        - type: Background
          data: Hoplite

p8_liberator:
  paragraphs:
    - text: |
        Египетская освободительница рабов Нубити начала свое приключение.
      env:
        - type: Background
          data: Liberator