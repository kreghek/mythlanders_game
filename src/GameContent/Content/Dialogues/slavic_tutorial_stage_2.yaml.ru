---
root:
  paragraphs:
    - text: 'Ратник видит двух человек, окруженных монстрами. Но эти бойцы и не думают сдаваться.'
    - speaker: Swordsman
      text: '"Собери стаю. Вот про что молвил тот странный Хорт.", тихо говорит Беримир себе под нос.'
      env:
        - type: PlayMusic
          data: Battle
    - speaker: Partisan
      text: '"Эй, приятель, не смотри на меня, просто поддержи меня. Наша борьба еще не окончена!"'
    - speaker: Robber
      text: '"Знаете, в моих любимых сказках всегда присутствует хитрый разбойник. Однако, он не всегда покидает передряги с... неповрежденными крыльями. Может быть, в этот раз моя удача улыбнется мне. И я буду не только хитрым, но еще и беззаботным негодяем?"'
      env:
        - type: MeetHero
          data: Partisan
        - type: MeetHero
          data: Robber
        - type: Trigger
          data: stage_2_meet_heroes
