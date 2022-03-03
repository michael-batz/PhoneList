<html>
    <head>
        <title>phone list - alphabetical</title>
        <style>
            body {
                font-family: Arial, Helvetica;
            }

            table {
                width: 100%;
                border: solid 1px;
                border-spacing: 0;
            }
            
            td {
                font-size: 10pt;
                border-top: solid 1px;
                border-bottom: solid 1px;
            }

            th {
                background-color: #222222;
                color: #FFFFFF;
                text-align: left;
            }

            .title {
                margin-top: 200px;
                page-break-after: always;
            }
            .title h1 {
                margin-top: 50px;
                margin-left: 70px;
            }

            .title p {
                margin-top: 500px;
                margin-left: 70px;
            }

            h2 {
                margin-top: 50px;
            }


            @page {
                @bottom-right {
                    content: "page " counter(page) " / " counter(pages);
                }
                font-family: Arial, Helvetica;
                font-size: 8pt;
            }
        </style>
    </head>
    <body>
        <div class="title">
            <h1>phone list<br />alphabetical</h1>
            <p>published: {{ date.now.day }}.{{ date.now.month }}.{{ date.now.year }}</p>
        </div>
        <table class="phonenumbers">
        {{for contact in contacts}}
            <tr>
                <td>{{ contact.last_name }}, {{ contact.first_name }}</td>
                <td style="width: 90px;">{{ department }}</td>
                <td style="width: 90px;">{{ contact.room }}</td>
                <td style="width: 120px;">{{ contact.phone_number }}</td>
            </tr>
        {{end}}
        </table>
    </body>
</html>
