using CheckpointTerminal.Http;
using CheckpointTerminal.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CheckpointTerminal.Classes
{
    public class UpdateData
    {
        public async Task<bool> Update()
        {
            try
            {
                HttpQuery httpManager = new HttpQuery();
                var allCheckpointTask = httpManager.GetAllCheckpoints();

                // Ожидание завершения всех задач запросов данных
                await Task.WhenAll(allCheckpointTask);
                var allCheckpoint = allCheckpointTask.Result;

                // Проверка наличия данных во всех полученных значениях
                if (allCheckpoint == null)
                {
                    // Один из запросов вернул null, обновление данных не удалось
                    return false;
                }

                // Обновление данных в синглтонах
                AllCheckpointsSingleton.Instance.Checkpoints = allCheckpoint;

                // Проверка наличия данных в синглтонах
                if (AllCheckpointsSingleton.Instance.Checkpoints.Count > 0)
                {
                    return true; // Данные загружены и обновлены успешно
                }
                else
                {
                    // Данные не загружены или обновлены
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                return false;
                // Дополнительные действия по обработке ошибки...
            }
        }
    }
}
