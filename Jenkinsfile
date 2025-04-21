pipeline {
    agent any
    environment {
        IMAGE_NAME = 'letschat'
        IMAGE_TAG = 'latest'
    }
    stages {

     stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Run Tests') {
            steps {
                script {
                    // Assuming your test project is at LetsChat.Tests or similar
                    bat 'dotnet test --logger:"trx;LogFileName=test_results.trx" --results-directory:"TestResults"'
                }
            }
        }

        stage('Publish App') {
            steps {
                script {
                    // Already inside the folder with .csproj
                    bat 'dotnet publish -c Release -o publish'
                }
            }
        }
        stage('Build Docker Image') {
            steps {
                script {
                    // Build using Dockerfile in current directory, using current folder as context
                    bat "docker build -t ${IMAGE_NAME}:${IMAGE_TAG} -f LetsChat/Dockerfile ."
                }
            }
        }
        stage('Run Docker Container') {
            steps {
                script {
                    // Stop container if already exists
                    bat "docker rm -f letschat-container || exit 0"

                    // Run the container
                    bat "docker run -d -p 9090:80 --name letschat-container ${IMAGE_NAME}:${IMAGE_TAG}"
                }
            }
        }
    }
    post {
        always {
            // Clean up dangling containers and images
            bat 'docker container prune -f'
            bat 'docker image prune -f'
        }
    }
}
