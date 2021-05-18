node('docker-image-builder') {
    def app

    stage('Clone repository') {
        checkout scm
    }

    stage('Build image') {
       app = docker.build("ongoonku/conversation-planner")
    }

    stage('Test image') {
        sh 'docker run --rm ongoonku/conversation-planner echo "hello world!"'
    }

    stage('Push image') {
        docker.withRegistry('https://registry.hub.docker.com', 'docker-ongoonku') {
            app.push("${env.BUILD_NUMBER}")
            app.push("latest")
        }
    }
}