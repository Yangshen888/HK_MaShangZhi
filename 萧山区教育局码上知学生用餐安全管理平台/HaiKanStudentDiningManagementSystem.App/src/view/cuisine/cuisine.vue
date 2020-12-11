<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.cuisine.query.totalCount"
        :pageSize="stores.cuisine.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      style="width: 150px;"
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.cuisine.query.kw1"
                      placeholder="请输入菜品名称"
                      @on-search="handleSearchDispatch()"
                    ></Input>
                    <Select
                      v-model="stores.cuisine.query.kw"
                      style="width:200px"
                      @on-change="xz"
                      clearable
                      placeholder="请选择学校"
                      v-show="schoolshow1==1"
                    >
                      <Option
                        v-for="item in this.school"
                        :value="item.schoolUuid"
                        :key="item.schoolUuid"
                      >{{ item.schoolName }}</Option>
                    </Select>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    v-show="schoolshow1==0"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                  v-show="schoolshow1==0"
                >添加</Button>
                <Button
                  v-can="'import'"
                  icon="ios-cloud-upload"
                  type="success"
                  @click="handleImportCuisine"
                  title="导入"
                  v-show="schoolshow1==0"
                >导入</Button>
                <Button
                  v-can="'export'"
                  icon="ios-cloud-download-outline"
                  type="primary"
                  @click="handleExportCuisine('export')"
                  title="导出"
                  v-show="schoolshow1==0"
                >导出</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.cuisine.data"
          :columns="stores.cuisine.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <!-- <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button
                  type="error"
                  v-show="schoolshow1==0"
                  v-can="'deletes'"
                  size="small"
                  shape="circle"
                  icon="md-trash"
                ></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                v-show="schoolshow1==0"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="名称" prop="cuisineName">
            <Input v-model="formModel.fields.cuisineName" placeholder="请输入菜品名称" :maxlength="30" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="价格" prop="price">
            <Input v-model="formModel.fields.price" placeholder="请输入价格" :maxlength="6" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="主料" prop="ingredient">
            <!-- <Select v-model="model1" multiple style="width: 238px;" filterable 
                @on-open-change="doIngredientGet2">
              <Option
                v-for="item in inglist"
                :key="item.ingredientUuid"
                :value="item.ingredientUuid"
              >{{ item.foodName }}</Option>
            </Select> -->
            <Input v-model="formModel.fields.ingredient" placeholder="请输入主料" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="配料" prop="burdening">
            <!-- <Select v-model="model2" multiple style="width: 238px;" filterable @on-open-change="doIngredientGet2">
              <Option
                v-for="item in inglist"
                :key="item.ingredientUuid"
                :value="item.ingredientUuid"
              >{{ item.foodName }}</Option>
            </Select> -->
            <Input v-model="formModel.fields.burdening" placeholder="请输入配料"/>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="类别" prop="cuisineType">
            <Select v-model="formModel.type" @on-change="changetype">
              <Option
                v-for="item in formModel.cuisinetype"
                :value="item.type"
                :key="item.type"
              >{{item.type}}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="简介" prop="abstract">
            <Input
              type="textarea"
              :autosize="{minRows: 2,maxRows: 10}"
              v-model="formModel.fields.abstracts"
              placeholder="请输入简介"
            />
          </FormItem>
        </Row>
        <Row>
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon type="ios-eye-outline" @click.native="handleView(item.url)"></Icon>
                <Icon type="ios-trash-outline" @click.native="handleRemove(item.name)"></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
          <Divider dashed />
          <Upload
            ref="upload"
            :show-upload-list="false"
            :default-file-list="defaultList"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            :format="['jpg','jpeg','png']"
            :max-size="5120"
            :data="{uuid:this.$store.state.user.userGuid}"
            :on-format-error="handleFormatError"
            :on-exceeded-size="handleMaxSize"
            :before-upload="handleBeforeUpload"
            :headers="postheaders"
            type="drag"
            :action="actionurl"
            style="display: inline-block;width:58px;"
          >
            <div style="width: 58px;height:58px;line-height: 58px;">
              <Icon type="ios-camera" size="20"></Icon>
            </div>
          </Upload>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitConsumable">保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer title="详情" v-model="formModel1.opened" width="400" :mask-closable="false" :mask="false">
      <Form :model="formModel1.fields" ref="formdispatch1" label-position="left">
        <Row :gutter="16" v-show="schoolshow1==1">
          <FormItem label="学校">
            <Input v-model="formModel1.fields.schoolName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="名称">
            <Input v-model="formModel1.fields.cuisineName" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="价格">
            <Input v-model="formModel1.fields.price" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="主料" prop="ingredient">
            <!-- <Select v-model="model1" multiple style="width: 238px;" filterable :disabled="true">
              <Option
                v-for="item in inglist"
                :key="item.ingredientUuid"
                :value="item.ingredientUuid"
              >{{ item.foodName }}</Option>
            </Select> -->
            <Input v-model="formModel1.fields.ingredient" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="配料" prop="burdening">
            <!-- <Select v-model="model2" multiple style="width: 238px;" filterable :disabled="true">
              <Option
                v-for="item in inglist"
                :key="item.ingredientUuid"
                :value="item.ingredientUuid"
              >{{ item.foodName }}</Option>
            </Select> -->
            <Input v-model="formModel1.fields.burdening" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="类别" prop="cuisineType">
            <Input v-model="formModel1.fields.cuisineType" :readonly="true" />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="简介" prop="abstract">
            <Input
              type="textarea"
              :autosize="true"
              v-model="formModel.fields.abstract"
              :readonly="true"
            />
          </FormItem>
        </Row>
        <Row>
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon type="ios-eye-outline" @click.native="handleView(item.url)"></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button style="margin-left: 30px" icon="md-close" @click="formModel1.opened = false">取 消</Button>
      </div>
    </Drawer>
    <Drawer
      title="菜品信息导入"
      v-model="formimport.opened"
      width="500"
      :mask-closable="true"
      :mask="true"
    >
      <div>
        <input
          ref="inputer"
          id="upload"
          type="file"
          @change="importfxx"
          accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
        />
        <Button
          icon="ios-cloud-download"
          type="warning"
          @click="handleimportmodel"
          title="菜品信息导入模板下载"
        >菜品信息导入模板下载</Button>
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleimport"
          :disabled="importdisable"
        >导入</Button>

        <Tabs value="name1">
          <TabPane label="成功" name="name1" v-html="successmsg"></TabPane>
          <TabPane label="重复" name="name2" v-html="repeatmsg"></TabPane>
          <TabPane label="失败" name="name3" v-html="defaultmsg"></TabPane>
        </Tabs>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  CuisineList, //显示列表
  CuisineCreate, //新增
  CuisineGet, //获取选定信息
  CuisineEdit, //编辑
  batchCommand, //批量删除
  deleteDepartment, //单个删除
  CuisineImport, //导入
  //getRegistPicture, //附件上传
  CuisineExport, //导出
  IngredientGet, //食材
  IngredientGet2, //食材
  SchoolList,
  deletetoFile,
  getRegistPicture,
  getiscuisine,
} from "@/api/cuisine/cuisine";
import config from "@/config";
import { getToken } from "@/libs/util";

import {
  globalvalidateIDCardNoMust,
  globalvalidatepwd,
  globalvalidateIsNotEmpty,
} from "@/global/validate";
export default {
  name: "Cuisine",
  components: {
    DzTable,
  },
  data() {
    const iscuisine = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入菜品名称"));
      } else {
        getiscuisine(value).then((res) => {
          if (res.data.data == true && this.formModel.mode=="create") {
            callback(new Error("该菜品已存在"));
          } else {
            callback();
          }
        });
      }
    };
    return {
      //导入
      url: config.baseUrl.dev,
      importdisable: false,

      successmsg: "",
      repeatmsg: "",
      defaultmsg: "",
      formimport: {
        opened: false
      },
      //附件上传
      imgshow1: false,
      img1: "",
      img: [],

      uploadList: [],
      defaultList: [],
      actionurl: "",
      postheaders: "",
      imgName: "",
      loadingStatus: false,
      updisabled: false,
      visible: false,

      valids: false,
      school: [],
      schoolshow1: 0,
      schoolshow2: 0,
      schoolshow3: 0,
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        Import: { name: "Import", title: "导入" },
        export: { name: "export", title: "导出" }
      },
      inglist: [],
      model1: [],
      model2: [],
      stores: {
        cuisine: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {},
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "cuisineUuid" },
            { title: "菜品名称", key: "cuisineName", sortable: true },
            { title: "主料", key: "ingredient" },
            { title: "配料", key: "burdening" },
            { title: "简介", key: "abstract" ,ellipsis:true},
            { title: "价格", key: "price",width: 70 },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        }
      },
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        fields: {
          cuisineName: "",
          price: "",
          burdening: "",
          ingredient: "",
          abstracts: "",
          cuisineType: "",
          isDeleted: 0,
          accessory: "",
          schoolUuid: "",
          addPeople: "",
          cuisineUuid: ""
        },
        type: "荤菜",
        cuisinetype: [
          { type: "荤菜" },
          { type: "半荤菜" },
          { type: "素菜" },
          { type: "甜点" },
          { type: "其它" }
        ],
        rules: {
          cuisineName: [
            {
              validator: iscuisine,
              type: "string",
              required: true,
            },
          ],
          price: [{ required: true, message: "请输入菜品价格" }],
        },
      },
      formModel1: {
        opened: false,
        selection: [],
        fields: {
          cuisineName: "",
          price: "",
          burdening: "",
          ingredient: "",
          abstract: "",
          cuisineType: "",
          isDeleted: 0,
          accessory: "",
          schoolUuid: "",
          addPeople: "",
          schoolName: ""
        }
      }
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "新增菜品信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑菜品信息";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.cuisineUuid);
    } //删除
  },
  methods: {
    //页面加载
    loadDispatchList() {
      console.log("asjghsgcjh");
       console.log(this.$store.state.user.schoolguid);
      if (this.$store.state.user.schoolguid != null) {
        this.stores.cuisine.query.kw = this.$store.state.user.schoolguid;
      } else {
        this.schoolshow1 = 1;
        // this.schoolshow2=1;
        // this.schoolshow3=1;
      }

      CuisineList(this.stores.cuisine.query).then(res => {
        console.log(res.data.data);
        this.stores.cuisine.data = res.data.data;
        this.stores.cuisine.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.cuisine.query.currentPage = page;
      this.loadDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.cuisine.query.pageSize = pageSize;
      this.loadDispatchList();
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadDispatchList();
    },
    //刷新
    handleRefresh() {
      this.loadDispatchList();
    },
    //清空
    handleResetFormDispatch() {
     // this.$refs["formdispatch"].resetFields();
      this.$refs.formdispatch.resetFields();
      this.$refs.formdispatch1.resetFields();
      this.uploadList = [];
      this.formModel.fields.cuisineName = "";
      this.formModel.fields.price = "";
      this.formModel.fields.schoolUuid = "";
      this.formModel.type = "荤菜";
      this.formModel.fields.abstracts = "";
      this.formModel.fields.accessory = "";
      this.formModel.fields.ingredient = "";
      this.formModel.fields.burdening = "";
      this.formModel.fields.cuisineUuid = "";
      this.img1 = "";
      this.imgcopy1 = "";
      this.img = [];
      this.imgshow1 = false;
      this.model1 = [];
      this.model2 = [];
    },
    //右边删除（单个删除）
    handleDelete(row) {
      if (this.$store.state.user.schoolguid == null) {
        this.$Message.warning("请先登录学校账号");
        return;
      }
      this.doDelete(row.cuisineUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteDepartment(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //右上边删除按钮
    handleBatchCommand(command) {
      if (this.$store.state.user.schoolguid == null) {
        this.$Message.warning("请先登录学校账号");
        return;
      }
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        }
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    //类别选择
    changetype(e) {
      this.formModel.fields.cuisineType = e;
    },
    //食材
    doIngredientGet() {
      if (this.$store.state.user.schoolguid == null) {
        IngredientGet("").then(res => {
          this.inglist = res.data.data;
        });
      } else {
        IngredientGet(this.$store.state.user.schoolguid).then(res => {
          this.inglist = res.data.data;
        });
      }
    },
    doIngredientGet2() {
      if (this.$store.state.user.schoolguid == null) {
        IngredientGet2("").then(res => {
          this.inglist = res.data.data;
        });
      } else {
        IngredientGet2(this.$store.state.user.schoolguid).then(res => {
          this.inglist = res.data.data;
        });
      }
    },
    doSchoolList() {
      SchoolList().then(res => {
        this.school = res.data.data;
      });
    },
    xz(e) {
      this.stores.cuisine.query.kw = e;
      this.loadDispatchList();
    },
    //添加按钮
    handleShowCreateWindow() {
      if (this.$store.state.user.schoolguid == null) {
        this.$Message.warning("请先登录学校账号");
        return;
      }
      this.formModel.mode = "create";
      this.handleResetFormDispatch(); //清空表单
      this.doIngredientGet2();
      this.formModel.fields.cuisineType = this.formModel.type;
      this.formModel.opened = true;
    },
    //右边编辑
    async handleEdit(row) {
      if (this.$store.state.user.schoolguid == null) {
        this.$Message.warning("请先登录学校账号");
        return;
      }
      if (this.formModel.fields.accessory != null) {
        this.formModel.dFileName = "";
        this.$refs.upload.fileList = [
          { name: "", status: "finished", showProgress: false }
        ];
      }
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doIngredientGet();
      this.doLoadData(row.cuisineUuid);
    },
    //右边详情
    handleDetail(row) {
      this.formModel1.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doIngredientGet();
      this.doLoadData(row.cuisineUuid);
    },
    //查询当前行信息
    doLoadData(id) {
      CuisineGet(id).then(res => {
        if (res.data.code === 200) {
          console.log(res.data.data);
          let data = res.data.data[0];
          this.formModel.fields = data;
          this.formModel1.fields = data;
          this.formModel.type = data.cuisineType;
          this.formModel.fields.abstracts = data.abstract;
          this.formModel.fields.cuisineUuid = data.cuisineUuid;
          
          //this.model1 = data.ingredient.split(",");
          //this.model2 = data.burdening.split(",");
          console.log(res.data.data[0]);
          if (res.data.data[0].accessory != null) {
            let list = res.data.data[0].accessory.split(",");
            for (let i = 0; i < list.length; i++) {
              this.uploadList.push({
                url:
                  config.baseUrl.dev + "UploadFiles/RegistPicture/" + list[i],
                status: "finished",
                name: "UploadFiles/RegistPicture/" + list[i],
                fileName: list[i]
              });
            }
          }

          // if (data.accessory != null) {
          //   let imgage = data.accessory.split("|");
          //   if (imgage[0] != "") {
          //     for (let k = 0; k < imgage.length; k++) {
          //       this.img[k] = this.url + data.accessory.split("|")[k];
          //     }
          //     this.img1 = this.url + data.accessory.split("|")[0];
          //     this.imgcopy1 = data.accessory;
          //     this.imgshow1 = true;
          //   } else {
          //     this.img1 = "";
          //     this.imgcopy1 = "";
          //     this.imgshow1 = false;
          //   }
          // }
        }
      });
    },

    validateSchoolForm() {
      this.$refs["formdispatch"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          this.valids = true;
        }
      });
    },
    //保存按钮
    handleSubmitConsumable() {
      //this.formModel.fields.ingredient = this.model1.join(",");
      //this.formModel.fields.burdening = this.model2.join(",");
      // if (this.formModel.fields.ingredient == "") {
      //   this.$Message.warning("菜品主料不能为空!");
      //   return;
      // }
      // this.validateSchoolForm();
      this.$refs["formdispatch"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          this.formModel.fields.schoolUuid = this.$store.state.user.schoolguid;
          let reg = /^[^\s]+$/;
          if (!reg.test(this.formModel.fields.cuisineName)) {
            this.$Message.warning("菜品名称不合法!");
            return;
          }
          let reg1 = /^(([1-9]{1}\d*)|(0{1}))(\.\d{0,2})?$/;
          if (!reg1.test(this.formModel.fields.price)) {
            this.$Message.warning("价格格式不合法!");
            return;
          }
          if (
            this.formModel.fields.accessory == null ||
            this.formModel.fields.accessory == ""
          ) {
            this.$Message.warning("请上传图片");
            return;
          }
          if (this.formModel.mode === "create") {
            this.docreateDispatch();
          }
          if (this.formModel.mode === "edit") {
            this.doEditDispatch();
          }
        }
      });
    },
    //添加（保存）
    docreateDispatch() {
      //this.datadeal();
      console.log(2222222222);
      console.log(this.formModel.fields);
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      CuisineCreate(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑（保存）
    doEditDispatch() {
      //this.datadeal();
      console.log(3333333);
      console.log(this.formModel.fields);

      CuisineEdit(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //导入相关操作
    handleImportCuisine() {
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      this.$refs.inputer.value = "";
      this.isexitexcel = false;
      this.formimport.opened = true;
    },
    //下载模板
    handleimportmodel() {
      console.log(this.url);
      window.location.href =
        this.url + "UploadFiles/ImportModel/菜品信息模板.xlsx";
    },
    //导入
    importfxx(e) {
      let inputDOM = this.$refs.inputer;
      //声明一个FormDate对象
      let formData = new FormData();
      let file = inputDOM.files[0];
      let AllUpExt = ".xls|.xlsx|";
      let extName = file.name
        .substring(file.name.lastIndexOf("."))
        .toLowerCase();
      if (AllUpExt.indexOf(extName + "|") == "-1") {
        this.$refs.inputer.value = "";
        this.$Message.warning("文件格式不正确!");
      } else {
        if (file != null && file != undefined) {
          this.isexitexcel = true;
          formData.append("excelFile", file);
          this.exceldata = formData;
        } else {
          this.isexitexcel = false;
        }
      }
    },
    async handleimport() {
      this.importdisable = true;
      this.successmsg = "";
      this.repeatmsg = "";
      this.defaultmsg = "";
      if (this.isexitexcel) {
        await CuisineImport(this.exceldata).then(res => {
          if (res.data.code == 200) {
            this.time = res.data.data.time;
            this.successmsg = res.data.data.successmsg;
            this.repeatmsg = res.data.data.repeatmsg;
            this.defaultmsg = res.data.data.defaultmsg;
            this.loadDispatchList();
          } else {
            this.$Message.warning(res.data.message);
          }
          this.$refs.inputer.value = "";
          this.exceldata = new FormData();
          this.isexitexcel = false;
        });
      } else {
        this.$Message.warning("请选择文件");
      }
      this.importdisable = false;
    },
    //导出
    handleExportCuisine(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doCuisineExport(command);
        }
      });
    },
    doCuisineExport(command) {
      console.log(this.selectedRowsId.join(","));
      CuisineExport(this.selectedRowsId.join(",")).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          //this.loadDispatchList();
          this.formModel.selection = [];
          console.log(res);
          window.location.href = config.baseUrl.dev + res.data.data;
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },

    checkShow() {
      return this.formModel.mode === "show";
    },
    
    async showUpResult(response, file, fileList) {
      console.log(this.$refs.upload.fileList);
      console.log(1);
      console.log(response);
      console.log(file);
      console.log(fileList);
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        console.log(this.formModel.fields.accessory);
        if (this.formModel.fields.accessory != null) {
          if (this.formModel.fields.accessory.length > 0) {
            this.formModel.fields.accessory += ",";
          }
          this.formModel.fields.accessory += response.data.fname;
        } else {
          this.formModel.fields.accessory = response.data.fname;
        }
        await this.uploadList.push({
          url: config.baseUrl.dev + response.data.strpath.replace("\\", "/"),
          status: "finished",
          name: response.data.strpath,
          fileName: response.data.fname
        });
        // console.log(
        //   (this.$refs.upload.fileList[0].name = e.data.dataPath.split("\\")[1])
        // );
        // console.log(this.defaultfilelist);
        // if (this.departmentlist.length >= 1) {
        //   this.defaultfilelist = [
        //     { name: this.formModel.fields.name, url: e.data.path }
        //   ];
        // }
      } else {
        this.$Message.warning(response.message);
      }
    },
    toUpResult() {
      console.log(1111);
      console.log(this.$refs.upload.fileList);
      //console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
        // this.$refs.upload.clearFiles();
        // this.$refs.upload.push({})
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "The file format is incorrect",
        desc: "文件: " + file.name + " 不是png,jpg"
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "Exceeding file size limit",
        desc: "文件 " + file.name + " 太大,超过5M."
      });
    },
    handleBeforeUpload() {
      // const check = this.uploadList.length < 5;
      // if (!check) {
      //   this.$Notice.warning({
      //     title: "Up to five pictures can be uploaded."
      //   });
      // }
      // return check;
      return true;
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
      console.log(this.imgName);
    },
    handleRemove(file) {
      console.log(file);
      // const fileList = this.$refs.upload.fileList;
      // this.$refs.upload.fileList.splice(fileList.indexOf(file), 1);
      deletetoFile({ path: file }).then(res => {
        console.log(res);
        if (res.data.data == "200") {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join(",");
        } else {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join(",");
        }
      });
    }
  },
  mounted() {
    this.loadDispatchList(); //页面加载
    this.doSchoolList();
  },
  created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken()
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/Cuisine/Cuisine/UpLoad";
  }
};
</script>
<style>
.file1:hover + .fileimg1:hover {
  transform: scale(1.01, 1.01);
  box-shadow: 0 0 3px #1783ba;
}
.fileimg1 {
  width: 300px;
  height: 169px;
  float: left;
  z-index: 2;
}

.demo-upload-list {
  display: inline-block;
  width: 120px;
  height: 120px;
  text-align: center;
  line-height: 120px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.demo-upload-list img {
  width: 100%;
  height: 100%;
}
.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}
.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>