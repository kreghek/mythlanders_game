---
root:
  paragraphs:
    - text: '��� ���������� �����, ����� ������������ ����.'
  options:
    - text: '�����������'
      next: heal
      aftermaths:
        - type: RestAll
          data: 2
      description: |
        � ����-������, ��� ����� ����������� HP �� 2.
        ������ ����� ����� �������� � �����.
    - text: '����������'
      selectConditions:
        - type: Disabled
      description: |
        In Development
    - text: '�������� ������'
      selectConditions:
        - type: Disabled
      description: |
        In Development

heal:
  paragraphs:
    - text: '��� ����� ������������ HP �� 2. ������ ����� ����� �������.'