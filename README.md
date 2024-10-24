# Инструкция по настройке и запуску приложения c тестовым заданием (Служба доставки)

## Описание

Это консольное приложение для фильтрации заказов службы доставки на основании района города и времени доставки. Программа загружает данные о заказах из текстового файла, фильтрует их и логирует результаты в указанные файлы.

## Необходимые компоненты

- [ .NET SDK ](https://dotnet.microsoft.com/download) (рекомендуется версия 8.0)

## Подготовка данных

1. **Создание файла с заказами:**
   - Создайте текстовый файл `orders.txt` в директории проекта. Формат каждой строки должен быть следующим:
     ```
     OrderId;Weight;District;DeliveryTime
     ```
   - Пример файла `orders.txt`:
     ```
     1;10,5;Center;2024-10-24 12:00:00
     2;5,0;North;2024-10-24 12:05:00
     3;8,5;Center;2024-10-24 12:35:00
     ```
     Можно использовать уже существующий файл Orders.txt
   - Убедитесь, что не осталось пустых строк и лишних пробелов. Временная метка должна быть в формате `yyyy-MM-dd HH:mm:ss`.

2. **Параметры запуска приложения:**
   - Приложение принимает 4 параметра из командной строки:
     1. **_cityDistrict:** Название района, по которому вы хотите фильтровать заказы.
     2. **_firstDeliveryDateTime:** Время первой доставки в формате `yyyy-MM-dd HH:mm:ss`.
     3. **_deliveryLog:** Путь к файлу, в который будут записаны логи.
     4. **_deliveryOrder:** Путь к файлу, в который будут сохранены отфильтрованные заказы.

## Запуск приложения

1. **Откройте консоль и перейдите в директорию проекта.**

2. **Компилируйте проект с помощью команды:**
   ```
   dotnet build
   ```
   
3. **Запустите приложение, передавая параметры:**
   ```
   dotnet run "Center" "2024-10-24 12:00:00" "log.txt" "output.txt"
   ```
   Здесь:
    - `"Center"` — пример района доставки.
    - `"2024-10-24 12:00:00"` — пример времени первой доставки.
    - `"log.txt"` — путь для логирования.
    - `"output.txt"` — путь для сохранения отфильтрованных заказов.
   
 4. **Логирование**
    
     Все действия, связанные с доставкой, будут записаны в файл, указанный в параметре логирования. Вы сможете найти информацию о каждом отфильтрованном заказе, включая его номер, район и время доставки.
    
 5. **Примечания**
    
  Программа написана для использования на Windows с ru-RU локалью. Поэтому данные формата числа с плавающей точкой записываются через знак запятой. Для использования данных с точкой необходимо изменить следующий код:
   - При парсинге в методе `LoadOrders` вместо `double.TryParse(parts[1].Replace(',', '.'), ...)` добавить `double.TryParse()`, что позволяет корректно обрабатывать числа с точкой;
   - При сохранении в методе `SaveFilteredOrders` удалить `.Replace('.', ',')` для сохранения чисел со знаком точки, а не запятой.
  Программа не должна аварийно завершаться при некорректных данных. В случае ошибок вы получите соответствующие сообщения в консоли.
  Убедитесь, что для файлов _deliveryLog.txt и _deliveryOrder.txt установлен доступ на запись.
  Для успешного выполнения приложения введите корректные параметры и данные в соответствии с указанными правилами.

 6. **Проблемы и поддержка**
    
    Если у вас возникли проблемы, проверьте:
    - Формат данных в файле orders.txt.
    - Наличие прав на запись в файл логов и результирующий файл.
    - Сообщения об ошибках в консоли для диагностики.

**Лицензия**
Это приложение является открытым исходным кодом и предоставляется без каких-либо гарантий. Используйте его на свой страх и риск.

### Заключение

Эта инструкция охватывает все ключевые аспекты работы с вашим приложением, от настройки до выполнения и обработки возможных ошибок. Если у вас есть какие-либо дополнительные рекомендации или изменения, не стесняйтесь сообщить! 😊
