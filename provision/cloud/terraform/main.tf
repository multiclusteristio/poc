provider "aws" {
  region = var.region
}

resource "aws_vpc" "vpc" {
  count = var.vpc_count
  cidr_block = "10.${count.index}.0.0/16"
  tags = {
    Name = "${var.vpcs[count.index]}-vpc"
  }
}

resource "aws_subnet" "subnet" {
  count = var.vpc_count
  vpc_id            = aws_vpc.vpc[count.index].id
  cidr_block        = "10.${count.index}.0.0/24"
  availability_zone = "${var.region}"
  map_public_ip_on_launch=true
}

resource "tls_private_key" "ec2_key" {
  count = var.vpc_count
  algorithm = "RSA"
  rsa_bits  = 2048
}

resource "aws_key_pair" "ec2_key_pair" {
  count      = var.vpc_count
  key_name   = "key-${count.index}"
  public_key = tls_private_key.ec2_key[count.index].public_key_openssh
}

resource "local_file" "private_key" {
  count    = var.vpc_count
  content  = tls_private_key.ec2_key[count.index].private_key_pem
  filename = "${var.ssh_key_directory}/key-${count.index}.pem"
}

resource "aws_security_group" "sg" {
  count = var.vpc_count
  vpc_id = aws_vpc.vpc[count.index].id

  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

data "aws_ami" "ubuntu" {
  # most_recent = true
  # owners      = ["099720109477"]
  # filter {
  #   name   = "name"
  #   values = ["ubuntu/images/hvm-ssd/ubuntu-focal-20.04-amd64-server-*"]
  # }\

 most_recent = true

  filter {
    name   = "image-id"
    values = ["ami-0f0cdb93f836d86cc"]
  }

  filter {
    name   = "virtualization-type"
    values = ["hvm"]
  }

  owners = ["099720109477"]  
}

resource "aws_instance" "ec2_instance" {
  count         = var.vpc_count
  ami           = data.aws_ami.ubuntu.id
  instance_type = var.instance_type
  subnet_id     = aws_subnet.subnet[count.index].id
  key_name      = aws_key_pair.ec2_key_pair[count.index].key_name
  security_groups = [aws_security_group.sg[count.index].id]
  tags = {
    Name = "KIND-Cluster-${count.index}"
  }

  user_data = <<-EOF
              #!/bin/bash
              apt update -y
              apt install -y docker.io
              systemctl start docker
              usermod -aG docker ubuntu
              EOF
}

resource "null_resource" "ansible_inventory" {
  provisioner "local-exec" {
    command = <<-EOC
      echo "[kind_hosts]" > ../ansible/inventory
      %{ for i, instance in aws_instance.ec2_instance }
      echo "${instance.public_ip} ansible_user=ubuntu ansible_ssh_private_key_file=../keys/key-${i}.pem" >> ../ansible/inventory
      %{ endfor }
      ansible-playbook -i ../ansible/inventory ../ansible/deploy_kind.yml
      ansible-playbook -i ../ansible/inventory ../ansible/deploy_argocd.yml
    EOC
  }

  depends_on = [aws_instance.ec2_instance]
}

# resource "null_resource" "ansible_inventory" {
#   provisioner "local-exec" {
#     command = <<-EOC
#       echo "[kind_hosts]" > ../ansible/inventory
#       for i in {0..3}; do
#         echo "${aws_instance.ec2_instance[i].public_ip} ansible_user=ubuntu ansible_ssh_private_key_file=../keys/key-${i}.pem" >> ../ansible/inventory
#       done

#       # Run Ansible playbooks
#       ansible-playbook -i ../ansible/inventory ../ansible/deploy_kind.yml
#       ansible-playbook -i ../ansible/inventory ../ansible/deploy_argocd.yml
#     EOC
#   }
#   depends_on = [aws_instance.ec2_instance]
# }