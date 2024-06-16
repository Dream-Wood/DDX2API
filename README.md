
![Poster](panel.png)
<h1 id="ddx2api">DDX2API</h1>
<p>Этот репозиторий содержит код для серверного API, предназначенного для мобильного приложения-ассистента. Приложение помогает в создании планов тренировок и облегчает коммуникацию между тренерами и пользователями. API построен с использованием .NET 8 и C# Web API.</p>
<h2 id="возможности">Возможности</h2>
<ul>
<li>Регистрация и аутентификация пользователей</li>
<li>Получение и обновление профилей пользователей</li>
<li>Управление планами тренировок</li>
<li>Обеспечение чатов и обмена сообщениями между пользователями и тренерами</li>
</ul>
<h2 id="используемые-технологии">Используемые технологии</h2>
<ul>
<li>.NET 8</li>
<li>C#</li>
<li>Entity Framework Core с PostgreSQL</li>
<li>Swagger и Postman для документации API</li>
<li>SHA256 для хеширования и безопасности</li>
</ul>
<h2 id="начало-работы">Начало работы</h2>
<h3 id="требования">Требования</h3>
<ul>
<li>.NET 8 SDK</li>
<li>База данных PostgreSQL</li>
<li>IDE, такая как Visual Studio или Visual Studio Code</li>
</ul>
<h3 id="установка">Установка</h3>
<ol>
<li>Клонируйте репозиторий:</li>
</ol>
<blockquote>
<pre><code>git clone https://github.com/Dream-Wood/DDX2API.git
cd DDX2API
</code></pre>
</blockquote>
<ol start="2">
<li>Настройте базу данных PostgreSQL и обновите строку подключения в <code>appsettings.json</code>:</li>
</ol>
<blockquote>
<pre><code> 
    {
    "ConnectionStrings": {
        "DbConnection": "Host=your_host;Database=your_database;Username=your_username;Password=your_password"
    },
    "PublicKey": "your_public_key"
}
</code></pre>
</blockquote>
<ol start="3">
<li>Восстановите зависимости и запустите приложение:</li>
</ol>
<blockquote
<p>dotnet restore<br>
dotnet run</p>
</blockquote>
<h2 id="конечные-точки-api">Конечные точки API</h2>
<p><a href="https://documenter.getpostman.com/view/26018573/2sA3XQiMyW">POSTMAN DOC</a></p>
<h2 id="безопасность">Безопасность</h2>
<p>Все чувствительные данные хешируются с использованием SHA256 для обеспечения безопасности. Ключи авторизации используются для проверки запросов пользователей.</p>
<h2 id="документация">Документация</h2>
<p>Для документации API используется Swagger и Postman. При запуске приложения в режиме разработки документация доступна по адресу <code>/swagger</code>.</p>
<h2 id="лицензия">Лицензия</h2>
<p>Этот проект лицензирован на условиях лицензии MIT.</p>
<hr>
<p>Не забудьте обновить <code>appsettings.json</code> с соответствующей строкой подключения и публичным ключом для корректной работы.</p>

