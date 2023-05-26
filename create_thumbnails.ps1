# Задаем путь к директории и маску файлов
$dirPath = ".\Rpg\Content\Content\Sprites\GameObjects\Characters"
$fileMask = "Full.png"

# Получаем список файлов по маске в указанной директории и поддиректориях
$fileList = Get-ChildItem -Path $dirPath -Recurse -Include $fileMask

# Задаем размеры прямоугольника для вырезания фрагмента изображения
$rectWidth = 32
$rectHeight = 32

# Перебираем каждый файл из списка
foreach ($file in $fileList) {
    # Загружаем изображение из файла
    $image = [System.Drawing.Image]::FromFile($file.FullName)
	
	$metaFile = [System.IO.Path]::GetDirectoryName($file.FullName) + "\Auto.meta"
	if (Test-Path -Path $metaFile -PathType Leaf) {
		# Чтение координат из текстового файла
		$coordinates = Get-Content -Path $metaFile

		# Разделение координат по пробелу и преобразование в числа
		$x = [double]$coordinates.Split(" ")[0]
		$y = [double]$coordinates.Split(" ")[1]
	}
	else
	{
		$x = 58
		$y = 16
	}

    # Вырезаем прямоугольный фрагмент изображения по заданным размерам
    $rect = New-Object System.Drawing.Rectangle($x, $y, $rectWidth, $rectHeight)
    $croppedImage = $image.Clone($rect, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)

    # Сохраняем вырезанный фрагмент в отдельный PNG-файл с тем же именем, но другим расширением
    $newFileName = [System.IO.Path]::GetDirectoryName($file.FullName) + "\Thumbnail.png"
    $croppedImage.Save($newFileName, [System.Drawing.Imaging.ImageFormat]::Png)

    # Освобождаем ресурсы занятые изображением и вырезанным фрагментом
    $image.Dispose()
    $croppedImage.Dispose()
}

# Выводим сообщение об успешном завершении скрипта
# Write-Host "Файлы успешно обработаны и сохранены в формате PNG."