pipeline {
    agent any
    environment {
        IMAGE_NAME = 'letschat'
        IMAGE_TAG = 'latest'
    }
    stages {
        stage('Publish App') {
            steps {
                script {
                    bat 'dotnet publish -c Release -o .\\publish'
                }
            }
        }
        stage('Build Docker Image') {
            steps {
                script {
                    // Ensure the tag is correctly referenced
                    bat "docker build -t %IMAGE_NAME%:%IMAGE_TAG% ."
                }
            }
        }
        stage('Run Docker Container') {
            steps {
                script {
                    bat "docker run -d -p 8080:80 %IMAGE_NAME%:%IMAGE_TAG%"
                }
            }
        }
    }
    post {
        always {
            bat 'docker container prune -f'
            bat 'docker image prune -f'
        }
    }
}
