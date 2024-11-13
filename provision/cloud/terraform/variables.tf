variable "region" {
  description = "AWS region to deploy instances"
  default     = "me-central-1"
}

variable "instance_type" {
  description = "Instance type for EC2"
  default     = "t3.xlarge"
}

variable "vpc_count" {
  description = "Number of VPCs to create"
  default     = 1
}
variable "vpcs" {
  default = ["dxb-containers", "dxb-vms", "auh-containers", "auh-vms"]
}


variable "ssh_key_directory" {
  description = "Directory to store SSH key files"
  default     = "../keys"
}
