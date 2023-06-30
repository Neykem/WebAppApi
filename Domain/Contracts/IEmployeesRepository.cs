using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAppApi.Data.DataModels;

namespace WebAppApi.Domain.Contracts
{
    /// <summary>
    /// Интерфейс по предастовлению функционал репозитория сотрудников
    /// </summary>
    public interface IEmployeesRepository
    {
        /// <summary>
        /// Метод возвращяет список сотрудников
        /// </summary>
        /// <returns>Возвращает коллекцию сотрудников</returns>
        public Task<List<Employee>> GetEmployeesAsync();

        /// <summary>
        /// Метод возвращяет сотрудника по id
        /// </summary>
        /// <param name="id">id сотрудника</param>
        /// <returns>Возвращает сущность сотрудника</returns>
        public Task<Employee> GetEmployeeAsync(Guid id);

        /// <summary>
        /// Метод удаляет сотрудника по id
        /// </summary>
        /// <param name="employee">Сущность сотрудника для удаления</param>
        /// <returns>Возвращает true при успешном выполнений операций</returns>
        public Task<bool> DeleteEmployeeAsync(Employee employee);

        /// <summary>
        /// Метод удаление сотрудников
        /// </summary>
        /// <param name="employee">Коллекция сущностей сотрудников для удаления</param>
        /// <returns>Возвращает true при успешном выполнений операций</returns>
        public Task<bool> DeleteEmployeeRangeAsync(List<Employee> employees);

        /// <summary>
        /// Метод обновляет данные сотрудника
        /// </summary>
        /// <param name="employee">Сущность сотрудника для сопостовления и обновления</param>
        /// <returns>Возвращает true при успешном выполнений операций</returns>
        public Task<bool> UpdateEmployeeAsync(Employee employee);

        /// <summary>
        /// Метод обновляет данные сотрудников
        /// </summary>
        /// <param name="employee">Коллекция сущностей сотрудника для сопостовления и обновления</param>
        /// <returns>Возвращает true при успешном выполнений операций</returns>
        public Task<bool> UpdateEmployeeRangeAsync(List<Employee> employees);

        /// <summary>
        /// Метод для создания сотрудника
        /// </summary>
        /// <param name="employee">Сущность сотрудника для добавления</param>
        /// <returns>Возвращает true при успешном выполнений операций</returns>
        public Task<bool> CreateEmployeeAsync(Employee employee);
    }
}
