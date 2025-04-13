pipeline {
    agent any

    tools {
        dotnet 'dotnet-8' // Match the tool name in Jenkins config
    }

    stages {
        stage('Checkout') {
            steps {
                git 'https://github.com/el2toro/LetsChat.git'
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build --configuration Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                sh 'dotnet test --no-restore --no-build --verbosity normal'
            }
        }

        stage('Publish') {
            steps {
                sh 'dotnet publish -c Release -o ./publish'
            }
        }
    }

    post {
        success {
            archiveArtifacts artifacts: 'publish/**', fingerprint: true
        }
    }
}
