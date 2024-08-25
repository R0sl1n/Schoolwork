# CitySprinters

Følg denne veiledningen for å komme i gang

## Installasjon

1. **Klone Repositoriet**: Klone dette repositoriet til din lokale maskin ved å bruke:

    ```
    git clone https://github.com/Sysutvikling-NE04/Turistapplikasjon.git
    ```

2. **Naviger til Prosjektmappen**: Bytt mappe til prosjektets mappe:

    ```
    cd Turistapplikasjon
    ```

3. **Sett Opp Python Miljø**:

    - **Windows**:

        ```
        python -m venv venv
        venv\Scripts\activate
        ```

    - **macOS**:
        ```
        python3 -m venv venv
        source venv/bin/activate
        ```

4. **Installer Krav**: Etter å ha aktivert det virtuelle miljøet, installer de nødvendige Python-pakkene ved hjelp av pip:

    ```
    pip install -r requirements.txt
    ```
5. **Database**: Sørg for at docker kjører, kjør kommandoen i terminalen:
    ```
    docker-compose up -d
    ```
    Gå til phpMyAdmin i nettleseren: 
    - [http://localhost:8082]
    - Logg inn med root/test
    - Endre rettigheter for brukeren 'user' til ALL PRIVILEGES i Brukerkontoer-fanen
    - Logg ut og logg deretter inn med user/test
    - Importer 'turistdb_create_script.sql'
    - For eksempel data importer scriptet 'example_data_inserts.sql'

## Bruk

- **Kjør Flask-appen**: Start Flask-appen ved å kjøre følgende kommando:

    ```
    python __init__.py
    ```

## Lisens

[MIT Lisens](LICENSE)

