---
root:
  paragraphs:
    - text: 'Это безопасное место, чтобы восстановить силы.'
  options:
    - text: 'Подлечиться'
      next: heal
      aftermaths:
        - type: RestAll
          data: 2
      description: |
        В демо-версии, все герои восстановят HP на 2.
        Павшие герои снова вернутся в строй.
    - text: 'Пообщаться'
      selectConditions:
        - type: Disabled
      description: |
        In Development
    - text: 'Осмотреь округу'
      selectConditions:
        - type: Disabled
      description: |
        In Development

heal:
  paragraphs:
    - text: 'Все герои восстановили HP на 2. Павшие герои снова активны.'