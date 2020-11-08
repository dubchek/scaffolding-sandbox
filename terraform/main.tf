# Specify the provider and access details
provider "aws" {
  region     = ""
  access_key = "${var.aws-access-key}"
  secret_key = "${var.aws-secret-key}"
}

 # Default vpc
resource "aws_vpc" "default" {
  cidr_block = "10.0.0.0/16"
}

# Default security group to access
# the instances over SSH and HTTP
resource "aws_security_group" "web" {
#  name        = "demo-security-group-from-terrorform" #optional, when omitted, terraform creates a random name
  description = "security group for demo web created from terraform"
  vpc_id      = "${aws_vpc.default.id}"

  # SSH access from anywhere
  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # HTTP access from the VPC
  ingress {
    from_port   = 8000
    to_port     = 8000
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # HTTP access from the VPC
  ingress {
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  # outbound internet access
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Our default security group to for the database
resource "aws_security_group" "mongo" {
  description = "security group created from terraform"
  vpc_id      = "vpc-c422e2a0"

  # SSH access from anywhere
  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  
  # mongodb access from anywhere
  ingress {
    from_port   = 27017
    to_port     = 27017
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # outbound internet access
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_instance" "mongodb" {
  # The connection block tells our provisioner how to
  # communicate with the resource (instance)
  connection {
    # The default username for our AMI
    user = "ubuntu"
  }

  instance_type = "t2.micro"
  
  tags = { Name = "mongodb instance" } 

  # standard realmethods community image with mongo started on the default port 
  ami = "ami-0e2a167cf2e0ce6c0"

  # The name of the  SSH keypair you've created and downloaded
  # from the AWS console.
  #
  # https://console.aws.amazon.com/ec2/v2/home?region=us-west-2#KeyPairs:
  #
  key_name = ""
  
  # Our Security group to allow HTTP and SSH access
  vpc_security_group_ids = ["${aws_security_group.mongo.id}"]
  
  # To ensure ssh access works
    provisioner "remote-exec" {
    inline = [
      "sudo ls",
    ]
  }

}

resource "aws_instance" "web" {
  # The connection block tells our provisioner how to
  # communicate with the resource (instance)
  connection {
    # The default username for our AMI
    user = "ubuntu"
 }

  instance_type = ""
  
  tags = { Name = "demo instance" } 

  # standard realmethods community AMI with docker pre-installed
  ami = "ami-05033408e5e831fb0"

  # The name of the  SSH keypair you've created and downloaded
  # from the AWS console.
  #
  # https://console.aws.amazon.com/ec2/v2/home?region=us-east-1#KeyPairs:
  #
  key_name = ""
  
  # Our Security group to allow HTTP and SSH access
  vpc_security_group_ids = ["${aws_security_group.web.id}"]

  provisioner "remote-exec" {
    inline = [
      "sudo apt-get -y update",
      "sudo apt-get -y install docker",
      "sudo docker login --username  --password ",
      "sudo docker pull /:",
      "sudo docker run -it -e DEFAULT_HOST_ADDRESS=${aws_instance.mongodb.public_ip} -e DEFAULT_HOST_PORT=27017 -p 8000:8000 -p 8080:8080 /:"
    ]
  }
}